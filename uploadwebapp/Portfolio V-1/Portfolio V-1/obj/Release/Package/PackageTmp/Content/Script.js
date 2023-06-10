// Sticky Navigation Menu Js

let nav = document.querySelector("nav");
let scrollBtn = document.querySelector(".scroll-button a");


let val;

window.onscroll = function () {
    if (document.documentElement.scrollTop > 20) {
        nav.classList.add("sticky");
        scrollBtn.style.display = "block";
    }
    else {
        nav.classList.remove("sticky");
        scrollBtn.style.display = "none";
    }
}
// Side Navigation Menu Js
//let body = document.querySelector("body");
//let navBar = document.querySelector(".navbar");
//let menuBtn = document.querySelector(".menu-btn");
//let cancelBtn = document.querySelector(".cancel-btn");

//menuBtn.onclick = function () {
//    navBar.classList.add("active");
//    menuBtn.style.opacity = "0";
//    menuBtn.style.pointerEvents = "none";
//    body.style.overflowX = "hidden";
//    scrollBtn.style.pointerEvents = "none";
//}

//cancelBtn.onclick = function () {
//    navBar.classList.remove("active");
//    menuBtn.style.opacity = "1";
//    menuBtn.style.pointerEvents = "auto";
//    body.style.overflowX = "auto";
//    scrollBtn.style.pointerEvents = "auto";
//}

//// Side Navigation Bar Close While We click On Navigation Links

//let navLinks = document.querySelectorAll(".menu li a");
//for (var i = 0; i < navLinks.length; i++) {
//    navLinks[i].addEventListener("click", function () {
//        navBar.classList.remove("active");
//        menuBtn.style.opacity = "1";
//        menuBtn.style.pointerEvents = "auto";
//    });
//}

//function openform() {
//    document.getElementById("myform").style.display = "block";
//}

//function closeform() {
//    document.getElementById("myform").style.display = "none";
//}

//typing animation script
var typed = new Typed(".typing", {
    strings: ["Computer Engineer", "Developer", "Designer", "Freelancer", "Gamer"],
    typespeed: 100,
    backspeed: 40,
    loop: true
});


/****************navbar********************/
// ---------Responsive-navbar-active-animation-----------
function test() {
	var tabsNewAnim = $('#navbarSupportedContent');
	var selectorNewAnim = $('#navbarSupportedContent').find('li').length;
	var activeItemNewAnim = tabsNewAnim.find('.active');
	var activeWidthNewAnimHeight = activeItemNewAnim.innerHeight();
	var activeWidthNewAnimWidth = activeItemNewAnim.innerWidth();
	var itemPosNewAnimTop = activeItemNewAnim.position();
	var itemPosNewAnimLeft = activeItemNewAnim.position();
	$(".hori-selector").css({
		"top": itemPosNewAnimTop.top + "px",
		"left": itemPosNewAnimLeft.left + "px",
		"height": activeWidthNewAnimHeight + "px",
		"width": activeWidthNewAnimWidth + "px"
	});
	$("#navbarSupportedContent").on("click", "li", function (e) {
		$('#navbarSupportedContent ul li').removeClass("active");
		$(this).addClass('active');
		var activeWidthNewAnimHeight = $(this).innerHeight();
		var activeWidthNewAnimWidth = $(this).innerWidth();
		var itemPosNewAnimTop = $(this).position();
		var itemPosNewAnimLeft = $(this).position();
		$(".hori-selector").css({
			"top": itemPosNewAnimTop.top + "px",
			"left": itemPosNewAnimLeft.left + "px",
			"height": activeWidthNewAnimHeight + "px",
			"width": activeWidthNewAnimWidth + "px"
		});
	});
}
$(document).ready(function () {
	setTimeout(function () { test(); });
});
$(window).on('resize', function () {
	setTimeout(function () { test(); }, 500);
});
$(".navbar-toggler").click(function () {
	$(".navbar-collapse").slideToggle(300);
	setTimeout(function () { test(); });
});



// --------------add active class-on another-page move----------
jQuery(document).ready(function ($) {
	// Get current path and find target link
	var path = window.location.pathname.split("/").pop();

	// Account for home page with empty path
	if (path == '') {
		path = 'index.html';
	}

	var target = $('#navbarSupportedContent ul li a[href="' + path + '"]');
	// Add active class to target link
	target.parent().addClass('active');
});




// Add active class on another page linked
// ==========================================
 $(window).on('load',function () {
     var current = location.pathname;
     console.log(current);
     $('#navbarSupportedContent ul li a').each(function(){
         var $this = $(this);
         // if the current path is like this link, make it active
         if($this.attr('href').indexOf(current) !== -1){
             $this.parent().addClass('active');
             $this.parents('.menu-submenu').addClass('show-dropdown');
             $this.parents('.menu-submenu').parent().addClass('active');
         }else{
             $this.parent().removeClass('active');
         }
     })
 });