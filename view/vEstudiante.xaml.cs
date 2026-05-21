using S6dchinchin.Modelos;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace S6dchinchin.view;

public partial class vEstudiante : ContentPage
{
    private const string URL = "http://10.2.0.248/moviles/post.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Estudiante> estudiantes;

    public vEstudiante()
    {
        InitializeComponent();
        Get();
    }

    public async void Get()
    {
        try
        {
            var content = await cliente.GetStringAsync(URL);

            List<Estudiante> objEstudiante =
                JsonConvert.DeserializeObject<List<Estudiante>>(content);

            estudiantes = new ObservableCollection<Estudiante>(objEstudiante);

            ListaEstudiantes.ItemsSource = estudiantes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}