<?php
/**
 * my_script.php
 *
 */
add_action( 'wp_enqueue_scripts', 'custom_scripts' );

function custom_scripts() {
    wp_enqueue_script( 'custom_script', get_stylesheet_directory_uri() . '/js/ie8.js', array('jquery'), '1.0' );
} ?>