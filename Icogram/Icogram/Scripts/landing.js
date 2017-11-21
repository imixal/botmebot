$(document).ready(function () {
    setupParalax();

    $(".slider").slick({
        autoplay: true,
        dots: true,
        responsive: [{
            breakpoint: 1000,
            settings: {
                dots: false,
                arrows: false,
                infinite: false,
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }]
    });
});

function setupParalax() {
    var $window = $(window);
    var $document = $(document);
    var $paralax = $('.paralax');

    function updatePositions() {
        var wTop = $window.scrollTop();
        var wHeight = $window.height();

        $paralax.each(function (i, p) {
            var $p = $(p);
            var pTop = $p.offset().top;
            var pHeight = $p.height();
            var perc = (pTop + pHeight - wTop) / wHeight * 100;
            $p.css('background-position-y', perc + '%');
        });
    }

    $(document).on('scroll', updatePositions);
    updatePositions();
}