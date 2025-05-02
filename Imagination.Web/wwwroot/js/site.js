function toggleLike(buttonElement) {
    var $button = $(buttonElement);
    var postId = $button.data("post-id");
    var likeCountSpan = $("#like-count-" + postId);

    $.ajax({
        url: "/Post/ToggleLike",
        type: "POST",
        data: { postId: postId },
        success: function (response) {
            if (response.success) {
                likeCountSpan.text(response.likeCount);

                if (response.isLiked) {
                    $button.removeClass("text-muted").addClass("text-danger");
                    $button.data("is-liked", "true");
                } else {
                    $button.removeClass("text-danger").addClass("text-muted");
                    $button.data("is-liked", "false");
                }
            }
        }
    });
}
function toggleReplyBox(link) {
    const replyBox = link.parentElement.querySelector('.reply-box');

    if (replyBox.classList.contains('d-none')) {
        replyBox.classList.remove('d-none');
    }
    else {
        replyBox.classList.add('d-none');
    }
}