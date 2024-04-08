window.initializeImageZoom = function () {
    $(document).ready(function () {
        $('#zoomImage').hover(
            function () {
                $(this).css("transform", "scale(1.2)"); // Scale up to 120% when hover
            },
            function () {
                $(this).css("transform", "scale(1)"); // Scale back to original size when not hovered
            }
        );
    });
}