$("#CPF").mask("000.000.000-00");
var tabela = $("#example").dataTable({
    "sDom": "<'dt-wrapper't><'dt-row dt-bottom-row'<'col-sm-6'i><'col-sm-6 text-right'p>>",
    "autoWidth": true,
    "iDisplayLength": 10,
    "aaData": [],
    "aoColumns": [
        { "sTitle": "ID", "mData": "Id", "sWidth": "10%" },
        { "sTitle": "Nome", "mData": "Nome", "sWidth": "30%" },
        { "sTitle": "Sobrenome", "mData": "Sobrenome", "sWidth": "35%" },
        { "sTitle": "CPF", "mData": "CPF", "sWidth": "15%" },
        {
            "sTitle": "Ações",
            "mData": null,
            "sWidth": "10%",
            "bSortable": false,
            "mRender": function (obj) {
                var settings = JSON.stringify({
                    "id": obj.Id,
                    "urlEditar": "/api/Cliente/EditarCliente",
                    "urlExcluir": "/api/Cliente/ExcluirCliente",
                    "seletorTabela": "#datatableProduto",
                    "seletorBotaoModalAtualizar": "#Atualizar",
                    "seletorFormulario": "#FormularioIncluir",
                    "seletorModal": "#myModal"
                });
                var botoes = criarBotoesTabelaDinamica(
                    "editarItemTabelaDinamica(this, " + settings + ")",
                    "excluirItemTabelaDinamica(this, " + settings + ")",
                    "#myModal");
                return botoes[0].outerHTML;
            }
        }
    ],
    "aaSorting": [],
    "oLanguage": {
        "sInfo": "_START_ a _END_ em _TOTAL_ " + "Clientes",
        "sInfoEmpty": "",
        "sEmptyTable": "Clique em novo para incluir novos clientes",
        "sInfoFiltered": "",
        "sZeroRecords": "Não há registros para o filtro informado",
        "oPaginate": {
            "sFirst": "<<",
            "sLast": ">>",
            "sPrevious": "<",
            "sNext": ">"
        }
    }
});

$("#Incluir").on("click", function () {
    $("#CPF").removeAttr("disabled");
    $("#FormularioIncluir input").each(function () {
        $(this).val("");
    });
    $("#Atualizar").hide();
    $("#Salvar").show();
    $("#myModal").modal("show");
});

$("#Salvar").off("click").on("click", function () {
    if ($("#Nome").val() !== "" && $("#Sobrenome").val() !== "" && $("#CPF").val() !== "") {
        var obj = { "Nome": $("#Nome").val(), "Sobrenome": $("#Sobrenome").val(), "CPF": $("#CPF").val() };
        $.ajax({
            url: "/api/Cliente/CriarCliente",
            data: obj,
            type: "POST",
            traditional: true
        }).success(function (data) {
            tabela.fnAddData([data]);
            $("#myModal").modal("hide");
        }).error(function (ex) {
            alert(ex.responseJSON.Message);
        });
    }
});

function criarBotoesTabelaDinamica(funcaoEditar, funcaoExcluir, seletorModal) {
    var div = $("<div>").attr({
        "class": "editable-buttons"
    });
    var botaoEditar = $("<a>").attr({
        "class": "btn btn-success btn-sm",
        "title": "Editar",
        "onclick": funcaoEditar,
        "data-toggle": "modal",
        "data-target": seletorModal
    }).append($("<i>").attr({
        "class": "glyphicon glyphicon-pencil"
    }));

    var botaoExcluir = $("<a>").attr({
        "class": "btn btn-danger btn-sm BotaoExcluir",
        "title": "Excluir",
        "onclick": funcaoExcluir
    }).append($("<i>").attr({
        "class": "glyphicon glyphicon-trash"
    }));

    div.append(botaoEditar);
    div.append(botaoExcluir);
    return div;
}

function preencherCamposModal(obj) {
    $("#Nome").val(obj["Nome"]);
    $("#Sobrenome").val(obj["Sobrenome"]);
    $("#CPF").val(obj["CPF"]).attr("disabled", "disabled");
}

function editarItemTabelaDinamica(tdCorrete, settings) {
    var $tr = $(tdCorrete).closest("tr");
    var obj = tabela.fnGetData($tr);
    preencherCamposModal.call(this, obj);
    $("#Salvar").hide();
    $("#Atualizar").show();
    $(settings.seletorBotaoModalAtualizar).off("click").on("click", function () {
        var $formulario = $(settings.seletorFormulario);
        var formularioDados = $formulario.serializeArray();
        if ($("#Nome").val() !== "" && $("#Sobrenome").val() !== "" && $("#CPF").val() !== "") {
            formularioDados.push({ "name": "Id", "value": settings.id });
            formularioDados.push({ "name": "CPF", "value": $("#CPF").val() });
            $.ajax({
                url: settings.urlEditar,
                data: formularioDados,
                type: "PUT",
                traditional: true
            }).success(function (data) {
                tabela.fnUpdate(data, $tr);
                $("#Atualizar").hide();
                $("#Fechar").click();
                $(".modal-backdrop.fade.in").remove();
            }).error(function (ex) {
                alert(ex.responseJSON.Message);
            });
        }
    });
}

function excluirItemTabelaDinamica(tdCorrete, settings) {
    $.ajax({
        url: settings.urlExcluir,
        data: { "Id": settings.id },
        type: "DELETE"
    }).success(function () {
        tabela.fnDeleteRow($(tdCorrete).closest("tr"));
    }).error(function (ex) {
        alert(ex.responseJSON.Message);
    });
}

$.ajax({
    url: "/api/Cliente/ConsultaClientes",
    type: "GET",
    traditional: true
}).success(function (data) {
    if (data.length) {
        tabela.fnAddData(data);
    }
}).error(function (ex) {
    alert(ex.statusText);
});