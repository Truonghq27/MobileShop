jQuery("#owl-slide").owlCarousel({
		items : 4, //10 items above 1000px browser width
		itemsDesktop : [1140,3], //5 items between 1024px and 901px
		itemsDesktopSmall : [986,2], // 3 items betweem 900px and 601px
		itemsTablet: [768,2], //2 items between 600 and 0;
		itemsMobile : [320,1],
		navigation : true,
		navigationText : ["<a class=\"flex-prev\"></a>","<a class=\"flex-next\"></a>"],
		slideSpeed : 500,
		
	});