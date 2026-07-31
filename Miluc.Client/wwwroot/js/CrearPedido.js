
window.mostrarEspera = function (
    titulo,
    descripcion,
    icono = "info"
) {

    Swal.fire({
        title: titulo,
        html: descripcion,
        icon: icono,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

};
// window.mostrarEspera = function (titulo, descripcion) {
//     Swal.fire({
//         title: titulo,
//         html: descripcion,
//         allowOutsideClick: false,
//         allowEscapeKey: false,
//         showConfirmButton: false,
//         didOpen: () => {
//             Swal.showLoading();
//         }
//     });
// };

window.cerrarEspera = function () {
    Swal.close();
};