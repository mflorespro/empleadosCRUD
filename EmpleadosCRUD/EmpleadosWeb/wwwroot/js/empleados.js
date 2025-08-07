document.addEventListener("DOMContentLoaded", function () {
    // Cargar el formulario en el modal al abrir
    const btnCrear = document.querySelector('button[data-bs-target="#modalCrearEmpleado"]');
    if (btnCrear) {
        btnCrear.addEventListener("click", function () {
            fetch("/Empleados/CreatePartial")
                .then(res => res.text())
                .then(html => {
                    document.getElementById("contenedorModalCrear").innerHTML = html;
                    const modalElement = document.getElementById("modalCrearEmpleado");
                    const modal = new bootstrap.Modal(modalElement);
                    modal.show();

                    // Configurar el evento submit del formulario
                    const formCrear = document.getElementById("formCrearEmpleado");
                    if (formCrear) {
                        formCrear.addEventListener("submit", function (e) {
                            e.preventDefault();
                            const button = e.target.querySelector("button[type=submit]");
                            button.disabled = true;

                            const data = Object.fromEntries(new FormData(formCrear));
                            fetch("/Empleados/Create", {
                                method: "POST",
                                headers: {
                                    "Content-Type": "application/json",
                                    "Accept": "application/json"
                                },
                                body: JSON.stringify(data)
                            })
                                .then(res => res.json())
                                .then(res => {
                                    button.disabled = false;
                                    if (res.success) {
                                        Swal.fire("¡Éxito!", "Empleado creado correctamente", "success")
                                            .then(() => {
                                                modal.hide();
                                                modal.dispose(); // Destruye la instancia
                                                const backdrop = document.querySelector(".modal-backdrop");
                                                if (backdrop) {
                                                    console.log("Eliminando backdrop");
                                                    backdrop.remove();
                                                } else {
                                                    console.log("No se encontró backdrop");
                                                }
                                                document.getElementById("contenedorModalCrear").innerHTML = "";
                                                actualizarTablaEmpleados();
                                            });
                                    } else {
                                        Swal.fire("Error", res.message || "No se pudo crear el empleado", "error");
                                    }
                                })
                                .catch(error => {
                                    button.disabled = false;
                                    Swal.fire("Error", "Error en la solicitud: " + error, "error");
                                });
                        });

                        // Manejar el botón Cancelar
                        const btnCancelar = document.querySelector(".btn-cancelar");
                        if (btnCancelar) {
                            btnCancelar.addEventListener("click", function () {
                                const modalElement = document.getElementById("modalCrearEmpleado");
                                const modal = bootstrap.Modal.getInstance(modalElement);
                                if (modal) {
                                    console.log("Cerrando modal");
                                    modal.hide();
                                    modal.dispose(); // Destruye la instancia
                                    const backdrop = document.querySelector(".modal-backdrop");
                                    if (backdrop) {
                                        console.log("Eliminando backdrop");
                                        backdrop.remove();
                                    } else {
                                        console.log("No se encontró backdrop");
                                    }
                                    document.getElementById("contenedorModalCrear").innerHTML = "";
                                }
                            });
                        }
                    }
                })
                .catch(error => console.error("Error al cargar el partial:", error));
        });
    }

    // Editar empleado
    document.querySelectorAll(".btnEditar").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;
            fetch(`/Empleados/ModalEditEmpleado?id=${id}`)
                .then(res => res.text())
                .then(html => {
                    document.getElementById("contenedorModal").innerHTML = html;
                    const modal = new bootstrap.Modal(document.getElementById("modalEditarEmpleado"));
                    modal.show();

                    const formEditar = document.getElementById("formEditarEmpleado");
                    formEditar.addEventListener("submit", function (e) {
                        e.preventDefault();
                        const data = Object.fromEntries(new FormData(formEditar));
                        fetch("/Empleados/EditarAjax", {
                            method: "POST",
                            headers: { "Content-Type": "application/json" },
                            body: JSON.stringify(data)
                        })
                            .then(res => res.json())
                            .then(res => {
                                if (res.success) {
                                    Swal.fire("¡Actualizado!", "Empleado editado correctamente", "success")
                                        .then(() => {
                                            modal.hide();
                                            modal.dispose();
                                            const backdrop = document.querySelector(".modal-backdrop");
                                            if (backdrop) {
                                                backdrop.remove();
                                            }
                                            document.getElementById("contenedorModal").innerHTML = "";
                                            actualizarTablaEmpleados();
                                        });
                                } else {
                                    Swal.fire("Error", "No se pudo editar el empleado", "error");
                                }
                            });
                    });
                });
        });
    });

    // Eliminar empleado
    document.querySelectorAll(".btnEliminar").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;
            Swal.fire({
                title: "¿Estás seguro?",
                text: "Esta acción no se puede deshacer",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then(result => {
                if (result.isConfirmed) {
                    fetch("/Empleados/EliminarAjax", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ id: id })
                    })
                        .then(res => res.json())
                        .then(res => {
                            if (res.success) {
                                Swal.fire("¡Eliminado!", "Empleado eliminado correctamente", "success")
                                    .then(() => actualizarTablaEmpleados());
                            } else {
                                Swal.fire("Error", "No se pudo eliminar el empleado", "error");
                            }
                        });
                }
            });
        });
    });

    // Actualizar tabla de empleados
    function actualizarTablaEmpleados() {
        fetch("/Empleados/ListarAjax")
            .then(res => res.json())
            .then(empleados => {
                const tbody = document.getElementById("tablaEmpleados");
                tbody.innerHTML = "";
                empleados.forEach(emp => {
                    tbody.innerHTML += `
                        <tr>
                            <td>${emp.id}</td>
                            <td>${emp.nombre}</td>
                            <td>${emp.apellido}</td>
                            <td>${emp.edad}</td>
                            <td>${emp.correo}</td>
                            <td>${emp.departamento}</td>
                            <td>${emp.puesto}</td>
                            <td>${emp.salario}</td>
                            <td>
                                <button class="btn btn-warning btnEditar" data-id="${emp.id}">Editar</button>
                                <button class="btn btn-danger btnEliminar" data-id="${emp.id}">Eliminar</button>
                            </td>
                        </tr>
                    `;
                });
                // Reiniciar DataTables si está inicializado
                if ($.fn.DataTable.isDataTable('#tablaEmpleados')) {
                    $('#tablaEmpleados').DataTable().destroy();
                    $('#tablaEmpleados').DataTable(); // Re-inicializar
                }
                // Reasignar eventos
                document.querySelectorAll(".btnEditar").forEach(btn => {
                    btn.addEventListener("click", function () {
                        const id = this.dataset.id;
                        fetch(`/Empleados/ModalEditEmpleado?id=${id}`)
                            .then(res => res.text())
                            .then(html => {
                                document.getElementById("contenedorModal").innerHTML = html;
                                const modal = new bootstrap.Modal(document.getElementById("modalEditarEmpleado"));
                                modal.show();

                                const formEditar = document.getElementById("formEditarEmpleado");
                                formEditar.addEventListener("submit", function (e) {
                                    e.preventDefault();
                                    const data = Object.fromEntries(new FormData(formEditar));
                                    fetch("/Empleados/EditarAjax", {
                                        method: "POST",
                                        headers: { "Content-Type": "application/json" },
                                        body: JSON.stringify(data)
                                    })
                                        .then(res => res.json())
                                        .then(res => {
                                            if (res.success) {
                                                Swal.fire("¡Actualizado!", "Empleado editado correctamente", "success")
                                                    .then(() => {
                                                        modal.hide();
                                                        modal.dispose();
                                                        const backdrop = document.querySelector(".modal-backdrop");
                                                        if (backdrop) {
                                                            backdrop.remove();
                                                        }
                                                        document.getElementById("contenedorModal").innerHTML = "";
                                                        actualizarTablaEmpleados();
                                                    });
                                            } else {
                                                Swal.fire("Error", "No se pudo editar el empleado", "error");
                                            }
                                        });
                                });
                            });
                    });
                });

                document.querySelectorAll(".btnEliminar").forEach(btn => {
                    btn.addEventListener("click", function () {
                        const id = this.dataset.id;
                        Swal.fire({
                            title: "¿Estás seguro?",
                            text: "Esta acción no se puede deshacer",
                            icon: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminar",
                            cancelButtonText: "Cancelar"
                        }).then(result => {
                            if (result.isConfirmed) {
                                fetch("/Empleados/EliminarAjax", {
                                    method: "POST",
                                    headers: { "Content-Type": "application/json" },
                                    body: JSON.stringify({ id: id })
                                })
                                    .then(res => res.json())
                                    .then(res => {
                                        if (res.success) {
                                            Swal.fire("¡Eliminado!", "Empleado eliminado correctamente", "success")
                                                .then(() => actualizarTablaEmpleados());
                                        } else {
                                            Swal.fire("Error", "No se pudo eliminar el empleado", "error");
                                        }
                                    });
                            }
                        });
                    });
                });
            })
            .catch(error => console.error("Error al actualizar la tabla:", error));
    }
});