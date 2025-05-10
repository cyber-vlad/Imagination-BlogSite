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
function toggleMainReplyBox(link) {
    const replyBox = link.parentElement.querySelector('.reply-box');
    if (replyBox) {
        replyBox.classList.toggle('d-none');
    }
}

$(document).ready(function () {
    $('form[id^="createCommentForm_"]').each(function () {
        var $form = $(this);

        $form.on('submit', function (e) {
            e.preventDefault();

            var postId = $form.data("post-id");
            var commentCountSpan = $("#comment-count-" + postId);

            var $commentInput = $form.find('[name="Content"]');
            var $submitButton = $form.find('button[type="submit"]');

            if ($commentInput.val().trim() === "") return;

            $submitButton.prop('disabled', true);

            $.ajax({
                url: $form.attr('action'),
                type: 'POST',
                data: $form.serialize(),
                success: function (response) {
                    if (response.success) {
                        if ($form.hasClass('refresh-on-submit')) {
                            location.reload();
                        } else {
                            $commentInput.val('');
                            commentCountSpan.text(response.commentCount);
                        }
                    }
                },
                complete: function () {
                    $submitButton.prop('disabled', false);
                }
            });
        });
    });
});

function createCommentBtn(buttonElement) {
    const postId = $(buttonElement).data('post-id');
    const form = $('#createCommentForm_' + postId);
    if (form.length) {
        form.find('input[name="Content"]').focus();
    }
}



function openParentReplyBox(commentId, username) {
    const replyBox = document.querySelector(`form[data-parent-comment-id="${commentId}"]`).closest('.reply-box');
    if (replyBox.classList.contains('d-none')) {
        replyBox.classList.remove('d-none');
    }

    const textarea = replyBox.querySelector("textarea");
    if (textarea && textarea.value.trim() === '') {
        textarea.value = `@${username} `;
        textarea.focus();
    }
}