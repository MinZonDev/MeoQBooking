(function($){
	"use strict";
	
	if( navigator.userAgent.match(/Android/i) || navigator.userAgent.match(/webOS/i) || 
		navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPad/i) || 
		navigator.userAgent.match(/iPod/i) || navigator.userAgent.match(/BlackBerry/i) || 
		navigator.userAgent.match(/Windows Phone/i) ){ 
		var gdlr_touch_device = true; 
	}else{ 
		var gdlr_touch_device = false; 
	}
	
	// retrieve GET variable from url
	$.extend({
	  getUrlVars: function(){
		var vars = [], hash;
		var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
		for(var i = 0; i < hashes.length; i++)
		{
		  hash = hashes[i].split('=');
		  vars.push(hash[0]);
		  vars[hash[0]] = hash[1];
		}
		return vars;
	  },
	  getUrlVar: function(name){
		return $.getUrlVars()[name];
	  }
	});	
	
	// blog - port nav
	function gdlr_set_item_outer_nav(){
		$('.blog-item-wrapper > .gdlr-nav-container').each(function(){
			var container = $(this).siblings('.blog-item-holder');
			var child = $(this).children();
			child.css({ 'top':container.position().top, 'bottom':'auto', height: container.height() - 50 });
		});
		$('.portfolio-item-wrapper > .gdlr-nav-container').each(function(){
			var container = $(this).siblings('.portfolio-item-holder');
			var child = $(this).children();
			child.css({ 'top':container.position().top, 'bottom':'auto', height: container.height() - 40 });
		});		
	}	
	
	// runs flex slider function
	$.fn.gdlr_flexslider = function(){
		if(typeof($.fn.flexslider) == 'function'){
			$(this).each(function(){
				var flex_attr = {
					animation: 'fade',
					animationLoop: true,
					prevText: '<i class="icon-angle-left" ></i>', 
					nextText: '<i class="icon-angle-right" ></i>',
					useCSS: false
				};
				
				// slide duration
				if( $(this).attr('data-pausetime') ){
					flex_attr.slideshowSpeed = parseInt($(this).attr('data-pausetime'));
				}
				if( $(this).attr('data-slidespeed') ){
					flex_attr.animationSpeed = parseInt($(this).attr('data-slidespeed'));
				}

				// set the attribute for carousel type
				if( $(this).attr('data-type') == 'carousel' ){
					flex_attr.move = 1;
					flex_attr.animation = 'slide';
					
					if( $(this).closest('.gdlr-item-no-space, .gdlr-full-size-wrapper').length > 0 ){
						flex_attr.itemWidth = $(this).width() / parseInt($(this).attr('data-columns'));
						flex_attr.itemMargin = 0;							
					}else{
						flex_attr.itemWidth = (($(this).width() + 30) / parseInt($(this).attr('data-columns'))) - 30;
						flex_attr.itemMargin = 30;	
					}		
					
					// if( $(this).attr('data-columns') == "1" ){ flex_attr.smoothHeight = true; }
				}else{
					if( $(this).attr('data-effect') ){
						flex_attr.animation = $(this).attr('data-effect');
					}
				}
				if( $(this).attr('data-columns') ){
					flex_attr.minItems = parseInt($(this).attr('data-columns'));
					flex_attr.maxItems = parseInt($(this).attr('data-columns'));	
				}				
				
				// set the navigation to different area
				if( $(this).attr('data-nav-container') ){
					var flex_parent = $(this).parents('.' + $(this).attr('data-nav-container')).prev('.gdlr-nav-container');
					
					if( flex_parent.find('.gdlr-flex-prev').length > 0 || flex_parent.find('.gdlr-flex-next').length > 0 ){
						flex_attr.controlNav = false;
						flex_attr.directionNav = false;
						flex_attr.start = function(slider){
							flex_parent.find('.gdlr-flex-next').on('click', function(){
								slider.flexAnimate(slider.getTarget("next"), true);
							});
							flex_parent.find('.gdlr-flex-prev').on('click', function(){
								slider.flexAnimate(slider.getTarget("prev"), true);
							});
							
							gdlr_set_item_outer_nav();
							$(window).on('resize', function(){ gdlr_set_item_outer_nav(); });
						}
					}else{
						flex_attr.controlNav = false;
						flex_attr.controlsContainer = flex_parent.find('.nav-container');	
					}
					
				}
				
				// set control nav for portfolio single
				if( $(this).closest('.gdlr-single-room-thumbnail').length > 0 ){
					flex_attr.manualControls = '#gdlr-flex-thumbnail-control li img';
				}

				$(this).flexslider(flex_attr);	
			});	
		}
	}
	
	// runs nivo slider function
	$.fn.gdlr_nivoslider = function(){
		if(typeof($.fn.nivoSlider) == 'function'){
			$(this).each(function(){
				var nivo_attr = {};
				
				if( $(this).attr('data-pausetime') ){
					nivo_attr.pauseTime = parseInt($(this).attr('data-pausetime'));
				}
				if( $(this).attr('data-slidespeed') ){
					nivo_attr.animSpeed = parseInt($(this).attr('data-slidespeed'));
				}
				if( $(this).attr('data-effect') ){
					nivo_attr.effect = $(this).attr('data-effect');
				}

				$(this).nivoSlider(nivo_attr);	
			});	
		}
	}	
	
	// runs isotope function
	$.fn.gdlr_isotope = function(){
		if(typeof($.fn.isotope) == 'function'){
			$(this).each(function(){
				var layout = ($(this).attr('data-layout'))? $(this).attr('data-layout'): 'fitRows';
				if( layout == 'fitRows' ) return;
				
				// execute isotope
				var isotope_element = $(this);
				isotope_element.children('.clear').remove();
				isotope_element.isotope({
					layoutMode: layout
				});
				
				// resize event
				$(window).on('resize', function(){
					isotope_element.isotope();
				});				
			});	
		}
	}
	
	// runs fancybox function
	$.fn.gdlr_fancybox = function(){
		if(typeof($.fn.fancybox) == 'function'){
			var fancybox_attr = {
				nextMethod : 'resizeIn',
				nextSpeed : 250,
				prevMethod : false,
				prevSpeed : 250,	
				helpers : { media : {} }
			};
			
			if( typeof($.fancybox.helpers.thumbs) == 'object' ){
				fancybox_attr.helpers.thumbs = { width: 50, height: 50 };
			}

			$(this).fancybox(fancybox_attr);
		}	
	}
	
	// responsive video
	$.fn.gdlr_fluid_video = function(){
		$(this).find('iframe[src^="http://www.youtube.com"], iframe[src^="//www.youtube.com"],'  +
					 'iframe[src^="http://player.vimeo.com"], iframe[src^="//player.vimeo.com"]').each(function(){

			// ignore if inside layerslider
			if( $(this).closest('.ls-container, .master-slider').length <= 0 ){ 
				if( ($(this).is('embed') && $(this).parent('object').length) || $(this).parent('.fluid-width-video-wrapper').length ){ return; } 
				if( !$(this).attr('id') ){ $(this).attr('id', 'gdlr-video-' + Math.floor(Math.random()*999999)); }			
			
				var ratio = $(this).height() / $(this).width();
				$(this).removeAttr('height').removeAttr('width');
				try{
					$(this).wrap('<div class="gdlr-fluid-video-wrapper"></div>').parent().css('padding-top', (ratio * 100)+"%");
					$(this).attr('src', $(this).attr('src'));
				}catch(e){
					
				}
			}
		
		});	
	}
	
	// pie chart
	$.fn.gdlr_pie_chart = function(){
		if(typeof($.fn.easyPieChart) == 'function'){
			$(this).each(function(){
				var gdlr_chart = $(this);
				
				$(this).easyPieChart({
					animate: 1200,
					lineWidth: gdlr_chart.attr('data-linewidth')? parseInt(gdlr_chart.attr('data-linewidth')): 8,
					size: gdlr_chart.attr('data-size')? parseInt(gdlr_chart.attr('data-size')): 155,
					barColor: gdlr_chart.attr('data-color')? gdlr_chart.attr('data-color'): '#a9e16e',
					trackColor: gdlr_chart.attr('data-bg-color')? gdlr_chart.attr('data-bg-color'): '#f2f2f2',
					backgroundColor: gdlr_chart.attr('data-background'),
					scaleColor: false,
					lineCap: 'square'
				});

				// for responsive purpose
				if($.browser.msie && (parseInt($.browser.version) <= 8)) return;
				function limit_gdlr_chart_size(){
					if( gdlr_chart.parent().width() < parseInt(gdlr_chart.attr('data-size')) ){
						var max_width = gdlr_chart.parent().width() + 'px';
						gdlr_chart.css({'max-width': max_width, 'max-height': max_width});
					}				
				}
				limit_gdlr_chart_size();
				$(window).on('resize', function(){ limit_gdlr_chart_size(); });
			});
		}
	}
		
	$(document).on('ready', function(){
		
		// room thumbnail
		$('.gdlr-room-thumbnail > a, .gdlr-room-thumbnail-wrap').on('hover', function(){
			$(this).find('img').transition({ scale: 1.1, duration: 200 });
		}, function(){
			$(this).find('img').transition({ scale: 1, duration: 200 });
		});

		// room modern new
		$('.gdlr-modern-room-new .gdlr-room-thumbnail-wrap').on('hover', function(){
			var overlay_item = $(this).children('.gdlr-room-thumbnail-overlay');
			var top_pos = overlay_item.outerHeight();
			overlay_item.css({top: 'auto'});
			top_pos -= overlay_item.outerHeight();
			
			overlay_item.css({top: 0});
			overlay_item.animate({top: top_pos}, {duration: 200, queue: false});
		}, function(){
			$(this).children('.gdlr-room-thumbnail-overlay').animate({top: 0}, {duration: 200, queue: false});
		});		

		// video responsive
		$('body').gdlr_fluid_video();	
	
		// top woocommerce button
		$('.gdlr-top-woocommerce-wrapper').on('hover', function(){
			$(this).children('.gdlr-top-woocommerce').fadeIn(200);
		}, function(){
			$(this).children('.gdlr-top-woocommerce').fadeOut(200);
		});
	
		// script for accordion item
		$('.gdlr-accordion-item').each(function(){
			var multiple_tab = $(this).hasClass('gdlr-multiple-tab');
			$(this).children('.accordion-tab').children('.accordion-title').on('click', function(){
				var current_tab = $(this).parent();
				if( current_tab.hasClass('active') ){
					current_tab.removeClass('pre-active');
					$(this).children('i').removeClass('icon-minus').addClass('icon-plus');
					$(this).siblings('.accordion-content').slideUp(function(){ current_tab.removeClass('active'); });
				}else{
					current_tab.addClass('pre-active');
					$(this).children('i').removeClass('icon-plus').addClass('icon-minus');	
					$(this).siblings('.accordion-content').slideDown(function(){ current_tab.addClass('active'); });
								
				}
				
				// close another tab if multiple tab is not allowed ( accordion )
				if( !multiple_tab ){
					current_tab.siblings().removeClass('pre-active');
					current_tab.siblings().children('.accordion-title').children('i').removeClass('icon-minus').addClass('icon-plus');
					current_tab.siblings().children('.accordion-content').slideUp(function(){ $(this).parent().removeClass('active'); });
				}
			});
		});
		
		// script for tab item
		$('.tab-title-wrapper').children().on('click', function(){
			$(this).addClass('active');
			$(this).siblings().removeClass('active');
			
			var selected_index = $(this).index() + 1;
			$(this).parent().siblings('.tab-content-wrapper').children(':nth-child(' + selected_index + ')').each(function(){
				$(this).siblings().removeClass('active').hide();
				$(this).fadeIn(function(){ $(this).addClass('active'); });
			})
		});		
	
		// initiate the tab when the get tab variable is sent
		var inital_tab = $.getUrlVar('tab');
		if( inital_tab ){ $('#' + inital_tab.replace(',', ', #')).each(function(){ $(this).trigger('click'); }); }
		
		// script for code item
		$('.gdlr-code-item .gdlr-code-title').on('click', function(){
			var parent = $(this).parent();
			if( parent.hasClass('active') ){
				$(this).children('i').removeClass('icon-minus').addClass('icon-plus');
				$(this).siblings('.gdlr-code-content').slideUp(function(){
					parent.removeClass('active');
				});	
			}else{
				$(this).children('i').removeClass('icon-plus').addClass('icon-minus');
				$(this).siblings('.gdlr-code-content').slideDown(function(){
					parent.addClass('active');	
				});				
			}
		});		
		
		// script for parallax background
		$('.gdlr-parallax-wrapper').each(function(){
			if( $(this).hasClass('gdlr-background-image') ){
				var parallax_section = $(this);
				var parallax_speed = parseFloat(parallax_section.attr('data-bgspeed'));
				if( parallax_speed == 0 || gdlr_touch_device ) return;
				if( parallax_speed == -1 ){
					parallax_section.css('background-attachment', 'fixed');
					parallax_section.css('background-position', 'center center');
					return;
				}
					
				$(window).on('scroll', function(){
					// if in area of interest
					if( ( $(window).scrollTop() + $(window).height() > parallax_section.offset().top ) &&
						( $(window).scrollTop() < parallax_section.offset().top + parallax_section.outerHeight() ) ){
						
						var scroll_pos = 0;
						if( $(window).height() > parallax_section.offset().top ){
							scroll_pos = $(window).scrollTop();
						}else{
							scroll_pos = $(window).scrollTop() + $(window).height() - parallax_section.offset().top;
						}
						parallax_section.css('background-position', 'center ' + (-scroll_pos * parallax_speed) + 'px');
					}
				});			
			}else if( $(this).hasClass('gdlr-background-video') ){
				if(typeof($.fn.mb_YTPlayer) == 'function'){
					$(this).children('.gdlr-bg-player').mb_YTPlayer();
				}
			}else{
				return;
			}
		});	
		
		// runs superfish menu
		if(typeof($.fn.superfish) == 'function'){
			
			// create the mega menu script
			$('#gdlr-main-navigation .sf-mega > ul').each(function(){	
				$(this).children('li').each(function(){
					var current_item = $(this);
					current_item.replaceWith(
						$('<div />').addClass('sf-mega-section')
									.addClass(current_item.attr('data-column'))
									.attr('data-size', current_item.attr('data-size'))
									.html(  $('<div />').addClass('sf-mega-section-inner')
														.addClass(current_item.attr('class'))
														.attr('id', current_item.attr('id'))
														.html(current_item.html())
									)		
					);
				});
				$(this).replaceWith(this.innerHTML);
			});
			
			// make every menu same height
			$('#gdlr-main-navigation .sf-mega').each(function(){
				var sf_mega = $(this); $(this).show();
				
				var row = 0; var column = 0; var max_height = 0;
				sf_mega.children('.sf-mega-section').each(function(){
					if( column % 60 == 0 ){ 
						if( row != 0 ){
							sf_mega.children('[data-row="' + row + '"]').children('.sf-mega-section-inner').height( max_height - 50 );
							max_height = 0;
						}
						row++; $(this).addClass('first-column'); 
					}		
					
					$(this).attr('data-row', row);	
					column += eval('60*' + $(this).attr('data-size'));
				
					if( $(this).height() > max_height ){
						max_height = $(this).height();
					}
				});
				
				sf_mega.children('[data-row="' + row + '"]').children('.sf-mega-section-inner').height( max_height - 50 );		
			});
			
			$('#gdlr-main-navigation').superfish({
				speed: 'fast'
			});		
		}
		
		// responsive menu
		if(typeof($.fn.dlmenu) == 'function'){
			$('#gdlr-responsive-navigation').each(function(){
				$(this).find('.dl-submenu').each(function(){
					if( $(this).siblings('a').attr('href') && $(this).siblings('a').attr('href') != '#' ){
						var parent_nav = $('<li class="menu-item gdlr-parent-menu"></li>');
						parent_nav.append($(this).siblings('a').clone());
						
						$(this).prepend(parent_nav);
					}
				});
				$(this).dlmenu();
			});
		}	
		
		// gallery thumbnail type
		$('.gdlr-gallery-thumbnail').each(function(){
			var thumbnail_container = $(this).children('.gdlr-gallery-thumbnail-container');
		
			$(this).find('.gallery-item').on('click', function(){
				var selected_slide = thumbnail_container.children('[data-id="' + $(this).attr('data-id') + '"]');
				if(selected_slide.css('display') == 'block') return false;
			
				// check the gallery height
				var image_width = selected_slide.children('img').attr('width');
				var image_ratio = selected_slide.children('img').attr('height') / image_width;
				var temp_height = image_ratio * Math.min(thumbnail_container.width(), image_width);
				
				thumbnail_container.animate({'height': temp_height});
				selected_slide.fadeIn().siblings().hide();
				return false;
			});		

			$(window).on('resize', function(){ thumbnail_container.css('height', 'auto') });
		});

		// fancybox
		$('a[href$=".jpg"], a[href$=".png"], a[href$=".gif"]').not('[data-rel="fancybox"], [href*="pinterest"]').attr('data-rel', 'fancybox');
		$('[data-rel="fancybox"]').gdlr_fancybox();
		
		// image shortcode 
		$('.gdlr-image-link-shortcode').on('hover', function(){
			$(this).find('.gdlr-image-link-overlay').animate({opacity: 0.8}, 150);
			$(this).find('.gdlr-image-link-icon').animate({opacity: 1}, 150);
		}, function(){
			$(this).find('.gdlr-image-link-overlay').animate({opacity: 0}, 150);
			$(this).find('.gdlr-image-link-icon').animate({opacity: 0}, 150);
		});	
		
		// Personnel
		$('.gdlr-personnel-item.round-style .personnel-item').each(function(){
			var current_item = $(this);
			function gdlr_set_round_personnel_height(){
				current_item.find('.personnel-item-inner').each(function(){
					$(this).css('margin-top', -($(this).height()/2));
				});
			}
			
			gdlr_set_round_personnel_height();
			$(window).on('resize', function(){
				gdlr_set_round_personnel_height();
			});
		});
		$('.gdlr-personnel-item.round-style .personnel-item').on('hover', function(){
			$(this).find('.personnel-author-image').animate({'opacity':0.05}, 200);
			$(this).find('.personnel-item-inner').animate({'opacity':1}, 200);
		}, function(){
			$(this).find('.personnel-author-image').animate({'opacity':1}, 200);
			$(this).find('.personnel-item-inner').animate({'opacity':0}, 200);
		});
		
		// Price Table
		$('.gdlr-price-table-item').each(function(){
			var price_table = $(this);
			
			function set_price_table_height(){
				var max_height = 0;
				var price_content = price_table.find('.price-content');
				
				price_content.css('height', 'auto');
				price_content.each(function(){
					if( max_height < $(this).height() ){ max_height = $(this).height(); }
				});
				price_content.css('height', max_height);
			}
			
			set_price_table_height()
			$(window).on('resize', function(){ set_price_table_height(); });
		});

		// Default text
		$('form').submit(function(){
			var has_default = false;
			$(this).find('input[data-default]').each(function(){
				if( $(this).attr('aria-required') != 'true' ){
					if( $(this).val() == $(this).attr('data-default') ) $(this).val('');
				}else{
					if( $(this).val() == $(this).attr('data-default') ) has_default = true;
				}
			});
			
			if(has_default) return false;
		});	
		
		// Search option
		$('#gdlr-menu-search-button').on('click', function(){
			$(this).siblings('#gdlr-menu-search').slideToggle(200);
		});
		$('.search-text input[data-default], .gdlr-comments-area input[data-default]').each(function(){
			var default_value = $(this).attr("data-default");
			$(this).val(default_value);
			$(this).on("blur", function(){
				if ($(this).val() == ""){
					$(this).val(default_value);
				}	
			}).on("focus", function(){
				if ($(this).val() == default_value){
					$(this).val("");
				}
			});		
		});		

		// smooth anchor
		if( window.location.hash ){
			$('html, body').animate({
				scrollTop: $(window.location.hash).offset().top - 68
			}, 500);
		}
		$('.gdlr-navigation a[href*="#"], .gdlr-responsive-navigation a[href*="#"]').on('click', function(){
			if( $(this).attr('href').length > 1 ){
				var item_id = $(this.hash);
				
				if( $('body').hasClass('home') ){
					if( item_id.length > 0 ){
						$('html, body').animate({
							scrollTop: item_id.offset().top - 68
						}, 500);
						return false;
					}
				}else{
					window.location.replace($('.body-wrapper').attr('data-home') + '/' + this.hash);
				}
			}
		});	
		
		// animate ux
		if( !gdlr_touch_device && ( !$.browser.msie || (parseInt($.browser.version) > 8)) ){
		
			// image ux
			$('.content-wrapper img').each(function(){
				if( $(this).closest('.gdlr-ux, .ls-wp-container, .product, .flexslider, .nivoSlider').length ) return;
				
				var ux_item = $(this);
				if( ux_item.offset().top > $(window).scrollTop() + $(window).height() ){
					ux_item.css({ 'opacity':1 });
				}else{ return; }
				
				$(window).on('scroll', function(){
					if( $(window).scrollTop() + $(window).height() > ux_item.offset().top + 100 ){
						ux_item.animate({ 'opacity':1 }, 1200); 
					}
				});					
			});
		
			// item ux
			$('.gdlr-ux').each(function(){
				var ux_item = $(this);
				if( ux_item.hasClass('gdlr-chart') || ux_item.hasClass('gdlr-skill-bar') ){
					if( ux_item.offset().top < $(window).scrollTop() + $(window).height() ){
						if( ux_item.hasClass('gdlr-chart') && (!$.browser.msie || (parseInt($.browser.version) > 8)) ){
							ux_item.gdlr_pie_chart();
						}else if( ux_item.hasClass('gdlr-skill-bar') ){
							ux_item.children('.gdlr-skill-bar-progress').each(function(){
								if($(this).attr('data-percent')){
									$(this).animate({width: $(this).attr('data-percent') + '%'}, 1200, 'easeOutQuart');
								}
							});	
						}
						return;
					}
				}else if( ux_item.offset().top > $(window).scrollTop() + $(window).height() ){
					ux_item.css({ 'opacity':1});
				}else{ return; }	

				$(window).on('scroll', function(){
					if( $(window).scrollTop() + $(window).height() > ux_item.offset().top + 100 ){
						if( ux_item.hasClass('gdlr-chart') && (!$.browser.msie || (parseInt($.browser.version) > 8)) ){
							ux_item.gdlr_pie_chart();
						}else if( ux_item.hasClass('gdlr-skill-bar') ){
							ux_item.children('.gdlr-skill-bar-progress').each(function(){
								if($(this).attr('data-percent')){
									$(this).animate({width: $(this).attr('data-percent') + '%'}, 1200, 'easeOutQuart');
								}
							});	
						}else{
							ux_item.animate({ 'opacity':1, 'padding-top':0, 'margin-bottom':0 }, 1200);
						}
					}
				});					
			});
			
		// do not animate on scroll in mobile
		}else{
		
			// Pie chart
			if(!$.browser.msie || (parseInt($.browser.version) > 8)){
				$('.gdlr-chart').gdlr_pie_chart();
			}	

		
			// skill bar
			$('.gdlr-skill-bar-progress').each(function(){
				if($(this).attr('data-percent')){
					$(this).animate({width: $(this).attr('data-percent') + '%'}, 1200, 'easeOutQuart');
				}
			});			
		}		

		// runs nivoslider when available
		$('.nivoSlider').gdlr_nivoslider();		
		
		// runs flexslider when available
		$('.flexslider').gdlr_flexslider();
		
	});
	
	$(window).on('load', function(){

		// run isotope when available
		$('.gdlr-isotope').gdlr_isotope();	
		
		// run pie chart for ie8 and below
		if($.browser.msie && (parseInt($.browser.version) <= 8)){
			$('.gdlr-chart').gdlr_pie_chart();
		}	
		
		// sliding nav
		if( !gdlr_touch_device ){
			var slide_nav = $('#gdlr-navigation-gimmick');
			var current_pos = 0;
			var current_menu_width = 0;
			
			$('#gdlr-main-navigation > ul > li.current-menu-item, #gdlr-main-navigation > ul > li.current-menu-ancestor, #gdlr-main-navigation > ul > li.current_page_item, #gdlr-main-navigation > ul > li.current_page_ancestor').each(function(){
				var top_pos = $(this).position().top + 28;
				var padding = parseInt($(this).children('a').css('padding-left'));
				current_pos = $(this).position().left + padding;
				
				padding += parseInt($(this).children('a').css('padding-right'));
				current_menu_width = $(this).width() - padding;

				slide_nav.css({'width': current_menu_width, 'left': current_pos, 'top':  top_pos});
			});

			$(window).on('resize', function(){
				$('#gdlr-main-navigation > ul > li.current-menu-item, #gdlr-main-navigation > ul > li.current-menu-ancestor, #gdlr-main-navigation > ul > li.current_page_item, #gdlr-main-navigation > ul > li.current_page_ancestor').each(function(){
					var top_pos = $(this).position().top + 28;
					var padding = parseInt($(this).children('a').css('padding-left'));
					current_pos = $(this).position().left + padding;
					
					padding += parseInt($(this).children('a').css('padding-right'));
					current_menu_width = $(this).width() - padding;

					slide_nav.css({'width': current_menu_width, 'left': current_pos, 'top':  top_pos});
				});
			});
				
			$('#gdlr-main-navigation > ul > li').on('hover', function(){
				var padding_left = parseInt($(this).children('a').css('padding-left'));
				var padding_right = parseInt($(this).children('a').css('padding-right'));
			
				slide_nav.animate({ 'width': jQuery(this).width() - (padding_left + padding_right), 'left': jQuery(this).position().left + padding_left}, 
					{ queue: false, easing: 'easeOutQuad', duration: 250 });			
			}, function(){
				slide_nav.animate({ 'width': current_menu_width, 'left': current_pos }, 
					{ queue: false, easing: 'easeOutQuad', duration: 250 });		
			});	
		}		

		// float menu
		$('.body-wrapper.float-menu').each(function(){
			var sub_area = $('#gdlr-header-substitute');
			var header_wrapper = sub_area.siblings('.gdlr-header-wrapper');
			var header_area = header_wrapper.children('.gdlr-header-inner');
			var header_style_1 = $('body').hasClass('header-style-1'); 
			
			var logo = header_area.find('.gdlr-logo');
			var logo_img = header_area.find('.gdlr-logo img');
			var navigation = header_area.find('.gdlr-navigation-wrapper');	
			var navigation_gimmick = $('#gdlr-navigation-gimmick');
			var original = {
				logo_top: logo.css('margin-top'), 
				logo_bottom: logo.css('margin-bottom'), 
				nav_top: navigation.css('margin-top'),
				gimmick_top: navigation_gimmick.css('top')
			};
				
			$(window).on('scroll', function(){
				if( header_area.hasClass('gdlr-fixed-header') && ($(this).width() < 959 || 
					$(this).scrollTop() <= header_wrapper.height()) ){
						if( logo_img.attr('data-trans') ){
							logo_img.attr('src', logo_img.attr('data-trans'));
						}
						
						var header_clone = header_area.clone(true);
						header_clone.appendTo($('body')).animate({opacity:0}, {duration: 300, queue: false, complete: function(){
							$(this).remove();
						}})
						
						header_wrapper.height('auto');
						header_area.appendTo(header_wrapper).removeClass('gdlr-fixed-header').css({opacity:1});
						
						if( header_style_1 ){
							logo.css({'margin-top': original.logo_top, 'margin-bottom': original.logo_bottom});
							logo_img.css({'width': '100%'});
						}else{
							logo.css({'display': 'block'});
						}
						navigation.css({'margin-top': original.nav_top});	

						navigation_gimmick.css({'top': original.gimmick_top});
				}else if( !header_area.hasClass('gdlr-fixed-header') && $(this).width() > 959 &&
					$(this).scrollTop() > header_wrapper.height() ){
						if( logo_img.attr('data-normal') ){
							logo_img.attr('data-trans', logo_img.attr('src'));
							logo_img.attr('src', logo_img.attr('data-normal'));
						}
						
						header_wrapper.height(header_wrapper.height());
						header_area.appendTo($('body')).addClass('gdlr-fixed-header').css({opacity:0}).animate({opacity:1}, {duration: 300, queue: false});
						
						navigation.css({'margin-top': '36px'});
						original.gimmick_top = navigation_gimmick.css('top');
						if( header_style_1 ){
							logo.css({'margin-top': '20px', 'margin-bottom': '23px'});
							logo_img.css({'width': '80%'});
							navigation_gimmick.css({'top': '66px'});
						}else{
							logo.css({'display': 'none'});
							navigation_gimmick.css({'top': '28px'});
						}
						
						
				}				
			});
		});			
		
		$(window).trigger('resize');
		$(window).trigger('scroll');
	});

})(jQuery);