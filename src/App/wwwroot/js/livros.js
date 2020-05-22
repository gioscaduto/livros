function Filtrar() {

    var pesquisa =
    {
        PalavraChave: $("#palavraChave").val(),
        DtPublicacaoInicial: $("#dtPublicacaoInicial").val(),
        DtPublicacaoFinal: $("#dtPublicacaoFinal").val(),
        Ordenacao: $("#ordenacao").val()
    }

    $.ajax({
        url: '/Livros/ObterListaLivros',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: { pesquisa: pesquisa },
        type: 'POST',
        success: function (partialView) {
            if (partialView) {
                $("#listaLivros").empty();
                $("#listaLivros").html(partialView);
            }
        },
        error: function (response) {
            console.log(response);
        }
    });

}