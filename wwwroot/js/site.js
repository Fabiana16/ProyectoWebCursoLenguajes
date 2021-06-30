// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//var r = document.getElementById("resta");
//var s = document.getElementById("suma");

//r.onclick = probar;
//s.onclick = probar;
//function probar() {
//    var x = document.getElementById("cantidad");
//    document.getElementById("demo").innerHTML = "You selected: " + x.value;

document.getElementById("cantidad").addEventListener("change", myFunction);

function myFunction() {
    var x = document.getElementById("cantidad");
    document.getElementById("demo").innerHTML = "You selected: " + x.value;
}

$('#myModal').on('shown.bs.modal', function () {
  $('#myInput').trigger('focus')
})

var input = document.getElementById('numCheq, banco');

function carg(elemento) {
    d = elemento.value;

    if (d == "Cheque") {
        numCheq.disabled = false;
        banco.disabled = false;
    } else {
        numCheq.disabled = true;
        banco.disabled = true;
    }
}

$('.alert').alert()