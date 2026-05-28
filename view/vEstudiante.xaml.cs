using S6dchinchin.Modelos;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace S6dchinchin.view;

public partial class vEstudiante : ContentPage
{
    private const string URL = "http://10.2.11.143/moviles/post.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Estudiante> estudiantes = new ObservableCollection<Estudiante>();

    public vEstudiante()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Get();
    }

    public async void Get()
    {
        try
        {
            var content = await cliente.GetStringAsync(URL);

            List<Estudiante> objEstudiante =
                JsonConvert.DeserializeObject<List<Estudiante>>(content) ?? new List<Estudiante>();

            estudiantes = new ObservableCollection<Estudiante>(objEstudiante);

            ListaEstudiantes.ItemsSource = estudiantes;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void ListaEstudiantes_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        if (e.Item is not Estudiante estudiante)
        {
            return;
        }

        ListaEstudiantes.SelectedItem = null;
        await Navigation.PushModalAsync(new vActElm(estudiante, estudiantes));
    }

    private async void BtnAgregar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new vRegistro(estudiantes));
    }
}
