using S6dchinchin.Modelos;
using System.Collections.ObjectModel;

namespace S6dchinchin.view;

public partial class vActElm : ContentPage
{
    private const string URL = "http://10.2.11.143/moviles/post.php";
    private readonly HttpClient cliente = new HttpClient();
    private readonly Estudiante estudiante;
    private readonly ObservableCollection<Estudiante> estudiantes;

    public vActElm(Estudiante estudianteSeleccionado, ObservableCollection<Estudiante> listaEstudiantes)
    {
        InitializeComponent();

        estudiante = estudianteSeleccionado;
        estudiantes = listaEstudiantes;

        txtId.Text = estudiante.id_estudiante.ToString();
        txtNombre.Text = estudiante.nombre;
        txtApellido.Text = estudiante.apellido;
        txtEdad.Text = estudiante.edad;
    }

    private async void BtnActualizar_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text))
        {
            await DisplayAlertAsync("Aviso", "Ingrese todos los datos.", "OK");
            return;
        }

        Estudiante estudianteActualizado = new Estudiante
        {
            id_estudiante = estudiante.id_estudiante,
            nombre = txtNombre.Text,
            apellido = txtApellido.Text,
            edad = txtEdad.Text
        };

        Dictionary<string, string> datos = new Dictionary<string, string>
        {
            { "accion", "actualizar" },
            { "action", "update" },
            { "id", estudianteActualizado.id_estudiante.ToString() },
            { "codigo", estudianteActualizado.id_estudiante.ToString() },
            { "id_estudiante", estudianteActualizado.id_estudiante.ToString() },
            { "nombre", estudianteActualizado.nombre },
            { "apellido", estudianteActualizado.apellido },
            { "edad", estudianteActualizado.edad }
        };

        try
        {
            string urlActualizar = $"{URL}?accion=actualizar&action=update&id={estudianteActualizado.id_estudiante}&codigo={estudianteActualizado.id_estudiante}&id_estudiante={estudianteActualizado.id_estudiante}&nombre={Uri.EscapeDataString(estudianteActualizado.nombre)}&apellido={Uri.EscapeDataString(estudianteActualizado.apellido)}&edad={Uri.EscapeDataString(estudianteActualizado.edad)}";
            FormUrlEncodedContent contenido = new FormUrlEncodedContent(datos);
            HttpResponseMessage respuesta = await cliente.PutAsync(urlActualizar, contenido);
            string mensaje = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Error", $"No se pudo actualizar en la base de datos.\n{mensaje}", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(mensaje))
            {
                await DisplayAlertAsync("Respuesta del servidor", mensaje, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
            return;
        }

        estudiante.nombre = estudianteActualizado.nombre;
        estudiante.apellido = estudianteActualizado.apellido;
        estudiante.edad = estudianteActualizado.edad;

        int index = estudiantes.IndexOf(estudiante);
        if (index >= 0)
        {
            estudiantes[index] = estudiante;
        }

        await DisplayAlertAsync("Correcto", "Registro actualizado.", "OK");
        await Navigation.PopModalAsync();
    }

    private async void BtnEliminar_Clicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlertAsync(
            "Eliminar",
            "Desea eliminar este registro?",
            "Si",
            "No");

        if (!confirmar)
        {
            return;
        }

        try
        {
            string urlEliminar = $"{URL}?codigo={estudiante.id_estudiante}";
            HttpResponseMessage respuesta = await cliente.DeleteAsync(urlEliminar);
            string mensaje = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Error", $"No se pudo eliminar en la base de datos.\n{mensaje}", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
            return;
        }

        estudiantes.Remove(estudiante);

        await DisplayAlertAsync("Correcto", "Registro eliminado.", "OK");
        await Navigation.PopModalAsync();
    }
}
