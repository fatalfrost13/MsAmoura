<?php /* Wrapper Name: Header */ ?>
<div class="row">
    <div class="span9" data-motopress-type="static" data-motopress-static-file="static/static-nav.php">
    	<?php get_template_part("static/static-nav"); ?>
    </div>
    <div class="span3 social-nets-wrapper" data-motopress-type="static" data-motopress-static-file="static/static-social-networks.php">
        <?php get_template_part("static/static-social-networks"); ?>
    </div>
</div>
<div class="row">
    <div class="span12 hidden-phone" data-motopress-type="static" data-motopress-static-file="static/static-search.php">
        <?php get_template_part("static/static-search"); ?>
    </div>
</div>
<div class="row">
    <div class="span6" data-motopress-type="static" data-motopress-static-file="static/static-logo.php">
    	<?php get_template_part("static/static-logo"); ?>
    </div>
    <div class="span6" data-motopress-type="dynamic-sidebar" data-motopress-sidebar-id="header-sidebar">
        <?php dynamic_sidebar("header-sidebar"); ?>
    </div>
</div>