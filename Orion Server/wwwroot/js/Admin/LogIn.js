var logInInput;
var logInBtn;

function LogIn()
{
    window.location.href = '/Admin/Index?id=' + logInInput.val();
}

$(function()
{
    logInInput = $('#logInInput');
    logInBtn = $('#logInBtn');

    logInInput.keypress(function (e)
    {
        if (e.which == 13)
            logInBtn.click();
    })
    logInBtn.click(LogIn);
});