// JavaScript source code

function get(id) {
    return document.getElementById(id);
}

function iniciarSesion() {
    var txtIniciarSesion = get("txtIniciarSesion");
    var txtPassword = get("txtPassword");

    if (txtIniciarSesion.value.trim() === "") {
        alert("Escriba su correo electr\u00F3nico.");
        return;
    }

    if (txtPassword.value.trim() === "") {
        alert("Escriba su contrase\u00F1a.");
        return;
    }
}

function registrarUsuario() {
    var txtNombre = get("txtNombre");
    var txtApellidos = get("txtApellidos");
    var txtCorreo = get("txtCorreo");
    var txtPassword = get("txtPassword");
    var txtConfirmarPassword = get("txtConfirmarPassword");

    if (txtNombre.value.trim() === "") {
        alert("Escriba su nombre completo.");
        return;
    }

    if (txtApellidos.value.trim() === "") {
        alert("Escriba sus apellidos.");
        return;
    }

    if (txtCorreo.value.trim() === "") {
        alert("Escriba su correo electr\u00F3nico.");
        return;
    }

    if (txtPassword.value.trim() === "") {
        alert("Escriba su contrase\u00F1a.");
        return;
    }

    if (txtConfirmarPassword.value.trim() === "") {
        alert("Confirme su contrase\u00F1a.");
        return;
    }
}