<?php /* Wrapper Name: Footer */ ?>
<?php if( is_front_page() ) { ?>
    <div class="line_ver_1">
        <div class="line_ver_2">
            <div class="row footer-widgets">
                <div class="span4" data-motopress-type="dynamic-sidebar" data-motopress-sidebar-id="footer-sidebar-1">
                    <?php dynamic_sidebar("footer-sidebar-1"); ?>
                </div>
                <div class="span3 offset1" data-motopress-type="dynamic-sidebar" data-motopress-sidebar-id="footer-sidebar-2">
                    <?php dynamic_sidebar("footer-sidebar-2"); ?>
                </div>
                <div class="span3 offset1" data-motopress-type="dynamic-sidebar" data-motopress-sidebar-id="footer-sidebar-3">
                    <?php dynamic_sidebar("footer-sidebar-3"); ?>
                </div>
            </div>
        </div>
    </div>
<?php } ?>
<div class="row">
    <div class="span12" data-motopress-type="static" data-motopress-static-file="static/static-footer-nav.php">
        <?php get_template_part("static/static-footer-nav"); ?>
    </div>
</div>
<div class="row">
    <div class="span12" data-motopress-type="static" data-motopress-static-file="static/static-footer-text.php">
        <div class="copyright">	
            <?php get_template_part("static/static-footer-text"); ?>
        </div>
    </div>
</div>