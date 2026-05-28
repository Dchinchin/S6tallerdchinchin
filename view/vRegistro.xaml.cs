using S6dchinchin.Modelos;
using System.Collections.ObjectModel;

namespace S6dchinchin.view;

public partial class vRegistro : ContentPage
{
    private const string URL = "http://10.2.11.143/moviles/post.php";
    private readonly HttpClient cliente = new HttpClient();
    private readonly ObservableCollection<Estudiante> estudiantes;

    public vRegistro(ObservableCollection<Estudiante> listaEstudiantes)
    {
        InitializeComponent();

        estudiantes = listaEstudiantes;
        txtId.Text = ObtenerNuevoId().ToString();
    }

    private int ObtenerNuevoId()
    {
        if (estudiantes.Count == 0)
        {
            return 1;
        }

        return estudiantes.Max(estudiante => estudiante.id_estudiante) + 1;
    }

    private async void BtnGuardar_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text))
        {
            await DisplayAlertAsync("Aviso", "Ingrese todos los datos.", "OK");
            return;
        }

        Estudiante nuevoEstudiante = new Estudiante
        {
            id_estudiante = int.Parse(txtId.Text ?? "0"),
            nombre = txtNombre.Text,
            apellido = txtApellido.Text,
            edad = txtEdad.Text
        };

        Dictionary<string, string> datos = new Dictionary<string, string>
        {
            { "codigo", nuevoEstudiante.id_estudiante.ToString() },
            { "id_estudiante", nuevoEstudiante.id_estudiante.ToString() },
            { "nombre", nuevoEstudiante.nombre },
            { "apellido", nuevoEstudiante.apellido },
            { "edad", nuevoEstudiante.edad }
        };

        try
        {
            FormUrlEncodedContent contenido = new FormUrlEncodedContent(datos);
            HttpResponseMessage respuesta = await cliente.PostAsync(URL, contenido);
            string mensaje = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Error", $"No se pudo guardar en la base de datos.\n{mensaje}", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
            return;
        }

        estudiantes.Add(nuevoEstudiante);
        await DisplayAlertAsync("Correcto", "Registro guardado.", "OK");
        await Navigation.PopModalAsync();
    }

    private async void BtnCancelar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
