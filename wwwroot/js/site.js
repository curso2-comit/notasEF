$(document).ready(function(){
    
    $('#consultarNotas').on('click', function(){
        $('#notas').empty();
        $('#notas').append('cargando...');
        $.ajax({
            url: '/home/consultarnotas',
            method: 'GET',
            success: function(response){
                $('#notas').empty();
                //mostramos notas
                for(var i=0;i<response.length;i++){
                    $('#notas').append('<div>' +
                        '<h2>' + response[i].titulo + '</h2>' +
                        '<p>' + response[i].cuerpo + '</p>' +
                        '<a href="/Notas/Editar/' + response[i].id +'">Editar</a><br>' +
                        '<a href="/Notas/Eliminar/' + response[i].id +'">Eliminar</a>' +
                    '</div>');                    
                }
                //agregamos botón de nueva nota
                $('#notas').append('<a id="agregarNota" href="#">Agregar nota</a>')
                console.log(response);
            },
            failure: function(error){
                console.log(error);
            }
        });
    });

    $('body').on('click', '#agregarNota', function(){
        $.ajax({
            url: '/home/crearnota',
            method: 'GET', //POST
            data: {
                titulo: 'Nota por Ajax',
                texto: 'El cuerpo de la nota por AJAX'
            },
            success: function(response){                
                console.log(response);

                $('#notas').append('<div>' +
                        '<h2>' + response.titulo + '</h2>' +
                        '<p>' + response.cuerpo + '</p>' +
                '</div>');
            },
            failure: function(error){
                console.log(error);
            }
        })
    })
});