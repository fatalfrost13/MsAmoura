<?php
	// Loading child theme textdomain
	load_child_theme_textdomain( CURRENT_THEME, get_stylesheet_directory() . '/languages' );

	// WP Pointers
	add_action('admin_enqueue_scripts', 'myHelpPointers');
	function myHelpPointers() {
	//First we define our pointers
	$pointers = array(
	   	array(
	       'id' => 'xyz1',   // unique id for this pointer
	       'screen' => 'options-permalink', // this is the page hook we want our pointer to show on
	       'target' => '#submit', // the css selector for the pointer to be tied to, best to use ID's
	       'title' => theme_locals("submit_permalink"),
	       'content' => theme_locals("submit_permalink_desc"),
	       'position' => array(
	                          'edge' => 'top', //top, bottom, left, right
	                          'align' => 'left', //top, bottom, left, right, middle
	                          'offset' => '0 5'
	                          )
	       ),

	    array(
	       'id' => 'xyz2',   // unique id for this pointer
	       'screen' => 'themes', // this is the page hook we want our pointer to show on
	       'target' => '#toplevel_page_options-framework', // the css selector for the pointer to be tied to, best to use ID's
	       'title' => theme_locals("import_sample_data"),
	       'content' => theme_locals("import_sample_data_desc"),
	       'position' => array(
	                          'edge' => 'bottom', //top, bottom, left, right
	                          'align' => 'top', //top, bottom, left, right, middle
	                          'offset' => '0 -10'
	                          )
	       ),

	    array(
	       'id' => 'xyz3',   // unique id for this pointer
	       'screen' => 'toplevel_page_options-framework', // this is the page hook we want our pointer to show on
	       'target' => '#toplevel_page_options-framework', // the css selector for the pointer to be tied to, best to use ID's
	       'title' => theme_locals("import_sample_data"),
	       'content' => theme_locals("import_sample_data_desc_2"),
	       'position' => array(
	                          'edge' => 'left', //top, bottom, left, right
	                          'align' => 'top', //top, bottom, left, right, middle
	                          'offset' => '0 18'
	                          )
	       )
	    // more as needed
	    );
		//Now we instantiate the class and pass our pointer array to the constructor
		$myPointers = new WP_Help_Pointer($pointers);
	};











//Recent Testimonials
if (!function_exists('shortcode_recenttesti')) {

	function shortcode_recenttesti($atts, $content = null) {
		extract(shortcode_atts(array(
				'num' => '5',
				'thumb' => 'true',
				'excerpt_count' => '30',
				'custom_class' => ''
		), $atts));

		$testi = get_posts('post_type=testi&orderby=post_date&numberposts='.$num);

		$output = '<div class="testimonials">';

		global $post;
		global $my_string_limit_words;

		foreach($testi as $post){
				setup_postdata($post);
				$excerpt = get_the_excerpt();
				$custom = get_post_custom($post->ID);
				if ( isset($custom["my_testi_caption"][0]) ) {
					$testiname = $custom["my_testi_caption"][0];
				}
				if ( isset($custom["my_testi_url"][0]) ) {
					$testiurl = $custom["my_testi_url"][0];
				}
				if ( isset($custom["my_testi_info"][0]) ) {
					$testiinfo = $custom["my_testi_info"][0];
				}

				$attachment_url = wp_get_attachment_image_src( get_post_thumbnail_id($post->ID), 'full' );
				$url = $attachment_url['0'];
				$image = aq_resize($url, 280, 240, true);

				$output .= '<div class="testi-item">';
						$output .= '<blockquote class="testi-item_blockquote">';
							if ($thumb == 'true') {
								if ( has_post_thumbnail($post->ID) ){
									$output .= '<figure class="featured-thumbnail">';
									$output .= '<img  src="'.$image.'"/>';
									$output .= '</figure>';
								}
							}
							$output .= '<a href="'.get_permalink($post->ID).'">';
								$output .= my_string_limit_words($excerpt,$excerpt_count);
							$output .= '</a><div class="clear"></div>';

					$output .= '</blockquote>';

					$output .= '<small class="testi-meta">';
							if( isset($testiname) ) {
								$output .= '<span class="user">';
									$output .= $testiname;
								$output .= '</span>';
							}

							if( isset($testiurl) ) {
								$output .= '<a href="'.$testiurl.'">';
									$output .= $testiurl;
								$output .= '</a>';
							}

						$output .= '</small>';

				$output .= '</div>';

		}
		$output .= '</div>';
		return $output;
	}
	add_shortcode('recenttesti', 'shortcode_recenttesti');

}

// Vertical line
function line_ver_shortcode($atts, $content = null) {

    $output = '<div class="line-ver">';
        $output .= do_shortcode($content);
    $output .= '</div>';

    return $output;
}
add_shortcode('line_ver', 'line_ver_shortcode');


/**
 * Post Cycle
 *
 */
if (!function_exists('shortcode_post_cycle')) {

	function shortcode_post_cycle($atts, $content = null) {
		extract(shortcode_atts(array(
				'num'              => '5',
				'type'             => 'post',
				'meta'             => '',
				'effect'           => 'slide',
				'thumb'            => 'true',
				'thumb_width'      => '200',
				'thumb_height'     => '180',
				'more_text_single' => theme_locals('read_more'),
				'category'         => '',
				'custom_category'  => '',
				'excerpt_count'    => '15',
				'pagination'       => 'true',
				'navigation'       => 'true',
				'custom_class'     => ''
		), $atts));

		$type_post         = $type;
		$slider_pagination = $pagination;
		$slider_navigation = $navigation;
		$random            = gener_random(10);
		$i                 = 0;
		$rand              = rand();

		$output = '<script type="text/javascript">
						jQuery(window).load(function() {
							jQuery("#flexslider_'.$random.'").flexslider({
								animation: "'.$effect.'",
								smoothHeight : true,
								directionNav: '.$slider_navigation.',
								controlNav: '.$slider_pagination.'
							});
						});';
		$output .= '</script>';
		$output .= '<div id="flexslider_'.$random.'" class="flexslider no-bg '.$custom_class.'">';
			$output .= '<ul class="slides">';

			global $post;
			global $my_string_limit_words;

			// WPML filter
			$suppress_filters = get_option('suppress_filters');

			$args = array(
				'post_type'              => $type_post,
				'category_name'          => $category,
				$type_post . '_category' => $custom_category,
				'numberposts'            => $num,
				'orderby'                => 'post_date',
				'order'                  => 'DESC',
				'suppress_filters'       => $suppress_filters
			);

			$latest = get_posts($args);

			foreach($latest as $key => $post) {
				// Unset not translated posts
				if ( function_exists( 'wpml_get_language_information' ) ) {
					global $sitepress;

					$check              = wpml_get_language_information( $post->ID );
					$language_code      = substr( $check['locale'], 0, 2 );
					if ( $language_code != $sitepress->get_current_language() ) unset( $latest[$key] );

					// Post ID is different in a second language Solution
					if ( function_exists( 'icl_object_id' ) ) $post = get_post( icl_object_id( $post->ID, $type_post, true ) );
				}
				setup_postdata($post);
				$excerpt        = get_the_excerpt();
				$attachment_url = wp_get_attachment_image_src( get_post_thumbnail_id($post->ID), 'full' );
				$url            = $attachment_url['0'];
				$image          = aq_resize($url, $thumb_width, $thumb_height, true);

				$output .= '<li>';

					if ($thumb == 'true') {

						if ( has_post_thumbnail($post->ID) ){
							$output .= '<figure class="thumbnail featured-thumbnail"><a href="'.get_permalink($post->ID).'" title="'.get_the_title($post->ID).'">';
							$output .= '<img  src="'.$image.'" alt="'.get_the_title($post->ID).'" />';
							$output .= '</a></figure>';
						} else {

							$thumbid = 0;
							$thumbid = get_post_thumbnail_id($post->ID);

							$images = get_children( array(
								'orderby'        => 'menu_order',
								'order'          => 'ASC',
								'post_type'      => 'attachment',
								'post_parent'    => $post->ID,
								'post_mime_type' => 'image',
								'post_status'    => null,
								'numberposts'    => -1
							) );

							if ( $images ) {

								$k = 0;
								//looping through the images
								foreach ( $images as $attachment_id => $attachment ) {
									// $prettyType = "prettyPhoto-".$rand ."[gallery".$i."]";
									//if( $attachment->ID == $thumbid ) continue;

									$image_attributes = wp_get_attachment_image_src( $attachment_id, 'full' ); // returns an array
									$img = aq_resize( $image_attributes[0], $thumb_width, $thumb_height, true ); //resize & crop img
									$alt = get_post_meta($attachment->ID, '_wp_attachment_image_alt', true);
									$image_title = $attachment->post_title;

									if ( $k == 0 ) {
										$output .= '<figure class="featured-thumbnail">';
										$output .= '<a href="'.get_permalink($post->ID).'" title="'.get_the_title($post->ID).'">';
										$output .= '<img  src="'.$img.'" alt="'.get_the_title($post->ID).'" />';
										$output .= '</a></figure>';
									} break;
									$k++;
								}
							}
						}
					}

					$output .= '<h5><a href="'.get_permalink($post->ID).'" title="'.get_the_title($post->ID).'">';
					$output .= get_the_title($post->ID);
					$output .= '</a></h5>';

					if($meta == 'true'){
						$output .= '<span class="meta">';
						$output .= '<span class="post-date">';
						$output .= get_the_time( get_option( 'date_format' ) );
						$output .= '</span>';
						$output .= '<span class="post-comments">'.theme_locals('comments').": ";
						$output .= '<a href="'.get_comments_link($post->ID).'">';
						$output .= get_comments_number($post->ID);
						$output .= '</a>';
						$output .= '</span>';
						$output .= '</span>';
					}
					//display post options
					$output .= '<div class="post_options">';
					switch($type_post) {
						case "team":
							$teampos  = (get_post_meta($post->ID, 'my_team_pos', true)) ? get_post_meta($post->ID, 'my_team_pos', true) : "";
							$teaminfo = (get_post_meta($post->ID, 'my_team_info', true)) ? get_post_meta($post->ID, 'my_team_info', true) : "";
							$output .= "<span class='page-desc'>".$teampos."</span><br><span class='team-content post-content'>".$teaminfo."</span>";
							$output .= cherry_get_post_networks(array('post_id' => $post->ID, 'display_title' => false, 'output_type' => 'return'));
							break;
						case "testi":
							$testiname = (get_post_meta($post->ID, 'my_testi_caption', true)) ? get_post_meta($post->ID, 'my_testi_caption', true) : "";
							$testiurl  = (get_post_meta($post->ID, 'my_testi_url', true)) ? get_post_meta($post->ID, 'my_testi_url', true) : "";
							$testiinfo = (get_post_meta($post->ID, 'my_testi_info', true)) ? get_post_meta($post->ID, 'my_testi_info', true) : "";
							$output .="<span class='user'>".$testiname."</span>, <span class='info'>".$testiinfo."</span><br><a href='".$testiurl."'>".$testiurl."</a>";
							break;
						case "portfolio":
							$portfolioClient = (get_post_meta($post->ID, 'tz_portfolio_client', true)) ? get_post_meta($post->ID, 'tz_portfolio_client', true) : "";
							$portfolioDate = (get_post_meta($post->ID, 'tz_portfolio_date', true)) ? get_post_meta($post->ID, 'tz_portfolio_date', true) : "";
							$portfolioInfo = (get_post_meta($post->ID, 'tz_portfolio_info', true)) ? get_post_meta($post->ID, 'tz_portfolio_info', true) : "";
							$portfolioURL = (get_post_meta($post->ID, 'tz_portfolio_url', true)) ? get_post_meta($post->ID, 'tz_portfolio_url', true) : "";
							$output .="<strong class='portfolio-meta-key'>".theme_locals('client').": </strong><span> ".$portfolioClient."</span><br>";
							$output .="<strong class='portfolio-meta-key'>".theme_locals('date').": </strong><span> ".$portfolioDate."</span><br>";
							$output .="<strong class='portfolio-meta-key'>".theme_locals('info').": </strong><span> ".$portfolioInfo."</span><br>";
							$output .="<a href='".$portfolioURL."'>".theme_locals('launch_project')."</a><br>";
							break;
						default:
							$output .="";
					};
					$output .= '</div>';

					if($excerpt_count >= 1){
						$output .= '<p class="excerpt">';
						$output .= my_string_limit_words($excerpt,$excerpt_count);
						$output .= '</p>';
					}

					if($more_text_single!=""){
						$output .= '<a href="'.get_permalink($post->ID).'" class="btn btn-primary" title="'.get_the_title($post->ID).'">';
						$output .= $more_text_single;
						$output .= '</a>';
					}

				$output .= '</li>';
			}
			wp_reset_postdata();
			$output .= '</ul>';
		$output .= '</div>';
		return $output;
	}
	add_shortcode('post_cycle', 'shortcode_post_cycle');
}

require_once('my_script.php');
?>