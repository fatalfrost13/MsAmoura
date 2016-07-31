include('js/jquery.easing.1.3.js');
include('js/jquery-ui-1.8.11.custom.min.js');
include('js/jquery.transform-0.9.3.min.js');
include('js/jquery.animate-colors-min.js');
include('js/jquery.backgroundpos.min.js');
include('js/mathUtils.js');
include('js/superfish.js');
include('js/switcher.js');
include('js/jquery.mousewheel.js');
include('js/sprites.js');
include('js/forms.js');
include('js/hoverSprite.js');
include('js/tms-0.4.1.js');
include('js/googleMap.js');
include('js/cScroll.js');

//----Include-Function----
function include(url){ 
  document.write('<script src="'+ url + '" type="text/javascript"></script>'); 
}
//--------global-------------
var isSplash = true;
var isIcon = true;
var isOver = false;
var MSIE = ($.browser.msie) && ($.browser.version <= 8)

function changeNum(num){
    img = num;
    $('.counter p').text((img+1)+'/'+$('.items li').length);
   
}

//------DocReady-------------
$(document).ready(function(){ 
    if(location.hash.length == 0){
        location.hash="!/"+$('#content > ul > li:first-child').attr('id');
    }

     $('ul#menu').superfish({
          delay:       800,
          animation:   {height:'show'},
          speed:       600,
          autoArrows:  false,
         dropShadows: false,
         	onInit: function(){
  				$("#menu > li > a").each(function(index){
  					var conText = $(this).find('.mText').text();
                       $(this).append("<div class='_area'></div><div class='_overPl'></div><div class='mTextOver'>"+conText+"</div>") 
                    		
  				})
  	 		}
        });
});
  
 //------WinLoad-------------  
$(window).load(function(){  
     
$(".prev, .next").hoverSprite({onLoadWebSite: true});
$('.more').sprites({method:'gStretch',hover:true});
$('.more2').sprites({method:'gStretch',hover:true});
  

 $('.scroll1').cScroll({
		duration:700,
		step:100,
		trackCl:'track',
		shuttleCl:'shuttle',
         hoverIn: function(_shuttle){
              //_shuttle.css({'backgroundPosition':'100% 0%'});    
        },
        hoverOut: function (_shuttle){
            //_shuttle.css({'backgroundPosition':'0% 0%'}); 
        }
	})   
    
     $('.scroll2').cScroll({
		duration:700,
		step:100,
		trackCl:'track2',
		shuttleCl:'shuttle2',
         hoverIn: function(_shuttle){
              //_shuttle.css({'backgroundPosition':'100% 0%'});    
        },
        hoverOut: function (_shuttle){
            //_shuttle.css({'backgroundPosition':'0% 0%'}); 
        }
	})  
    
    $('.scroll3').cScroll({
		duration:700,
		step:100,
		trackCl:'track3',
		shuttleCl:'shuttle3',
         hoverIn: function(_shuttle){
              //_shuttle.css({'backgroundPosition':'100% 0%'});    
        },
        hoverOut: function (_shuttle){
            //_shuttle.css({'backgroundPosition':'0% 0%'}); 
        }
	})

$('._list1 > li > a').hover(
	function(){
 		$(this).stop().animate({color:"#fff"},300)
   },
   function(){
   		$(this).stop().animate({color:"#8E8E8E"},300)
        }
    )

$('._list2 > li > a').hover(
	function(){
 		$(this).stop().animate({color:"#fff"},300)
   },
   function(){
   		$(this).stop().animate({color:"#8E8E8E"},300)
        }
    )

$('.more').hover(
	function(){
 		$(this).stop().animate({color:"#fff"},300)
   },
   function(){
   		$(this).stop().animate({color:"#000"},300)
        }
    )  

$('.more2').hover(
	function(){
 		$(this).stop().animate({color:"#000"},300)
   },
   function(){
   		$(this).stop().animate({color:"#fff"},300)
        }
    )    
  
       
var menuItems = $('#menu >li'); 
var currentIm = 0;
var lastIm = 0;

navInit();
function navInit(){
    var img=0;
    var num=$('.items li').length-1;
}

    $('.slider')._TMS({
		show:0,
		pauseOnHover:false,
		duration:800,
		preset:'diagonalFade',
        easing: 'easeOutExpo',
		pagination: false,
        prevBu: '.prev',
        nextBu: '.next',
		pagNums: false,
		slideshow: 9999999,
		numStatus:false,
		banners:false,
		overflow:'hidden',
		waitBannerAnimation:false,
		progressBar:false
	}); 
///////////////////////////////////////////////
    var navItems = $('.menu > ul >li');

   // $('.menu > ul >li').eq(0).css({'display':'none'});
	var content=$('#content'),
		nav=$('.menu');

    	$('#content').tabs({
		preFu:function(_){
			_.li.css({top:'242px', height:"0px", 'display':'none'});
		}
		,actFu:function(_){			
			if(_.curr){
				_.curr.css({'display':'block', height:'0px', top:'243px'}).stop().delay(400).animate({height:"428px"},800,'easeOutCubic');
                if ((_.n == 0) && ((_.pren>0) || (_.pren==undefined))){splashMode();}
                if (((_.pren == 0) || (_.pren == undefined)) && (_.n>0) ){contentMode(); }
            }
			if(_.prev){
			     _.prev.stop().animate({height:"0px", top:'450px'},600,'easeInOutCubic',function(){_.prev.css({'display':'none'});} );
             }
		}
	})
    

    function splashMode(){
        isSplash = true;
       
    }
    
    function contentMode(){  
        isSplash = false;

    }		
    
	nav.navs({
			useHash:true,
             hoverIn:function(li){
                $(".mText", li).stop(true).animate({top:"95px"}, 500, 'easeOutCubic');
                $(".mTextOver", li).stop(true).animate({top:"0px"}, 500, 'easeOutCubic');
                $("._overPl", li).stop(true).animate({top:"0px"}, 400, 'easeOutCubic');
              
                   // if(($.browser.msie) && ($.browser.version <= 8)){}else{}
             },
                hoverOut:function(li){
                    if ((!li.hasClass('with_ul')) || (!li.hasClass('sfHover'))) {
                        $(".mText", li).stop(true).animate({top:"0px"}, 500, 'easeOutBack');
                        $(".mTextOver", li).stop(true).animate({top:"-100px"}, 400, 'easeOutCubic');
                        $("._overPl", li).stop(true).animate({top:"80px"}, 500, 'easeOutCubic');
                    } 
                } 
		})

		.navs(function(n){			
			$('#content').tabs(n);
		})

//////////////////////////////////////////
   	var h_cont=885;
	function centrRepos() {
		var h=$(window).height();
		if (h>(h_cont+40)) {
			m_top=~~(h-h_cont)/2;
			h_new=h;
		} else {
			m_top=20;
			h_new=h_cont+40;
		}
		$('.center').stop().animate({paddingTop:m_top},800,'easeOutExpo');
        
     
   
	}
	centrRepos();
    ///////////Window resize///////
    
    function windowW() {
 return (($(window).width()>=parseInt($('body').css('minWidth')))?$(window).width():parseInt($('body').css('minWidth')));
}
    
    
	$(window).resize(function(){
        centrRepos();
         
        }
    );

    } //window function
) //window load