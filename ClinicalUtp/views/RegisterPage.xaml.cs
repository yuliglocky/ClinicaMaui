namespace ClinicalUtp.views;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
public partial class RegisterPage : ContentPage
{
    private readonly PacienteServices _registroService;
    public RegisterPage()
    {
        InitializeComponent();
        _registroService = new PacienteServices(new HttpClient());
    }

    private void OnDniTextChanged(object sender, TextChangedEventArgs e)
    {
        // Filtrar s�lo n�meros en el campo DNI
        dniEntry.Text = FilterNumericInput(e.NewTextValue);
    }

    private void OnCellphoneTextChanged(object sender, TextChangedEventArgs e)
    {
        // Filtrar s�lo n�meros en el campo celular
        cellphoneEntry.Text = FilterNumericInput(e.NewTextValue);
    }

    // Funci�n para filtrar s�lo n�meros
    private string FilterNumericInput(string input)
    {
        // Usar LINQ para eliminar caracteres no num�ricos
        return new string(input.Where(char.IsDigit).ToArray());
    } 

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Obtener los datos de los controles de la p�gina
        var nombre = nombreEntry.Text;
        var email = emailEntry.Text;
        var dni = dniEntry.Text;
        var celular = cellphoneEntry.Text;
        var fechaNacimiento = birthdayPicker.Date;
        var direccion = addressEntry.Text;
        var alergias = allergiesEntry.Text;
        var esDonante = isDonorSwitch.IsToggled;

        // Obtener el g�nero seleccionado
        var genderIndex = genderPicker.SelectedIndex;
        int gender = genderIndex == 0 ? 1 : genderIndex == 1 ? 2 : 0;  // Masculino: 1, Femenino: 2, Otro: 0

        // Obtener el tipo de sangre como texto
        var blood = bloodPicker.SelectedItem as string;

        // Verificar que el tipo de sangre est� seleccionado
        if (string.IsNullOrWhiteSpace(blood))
        {
            await DisplayAlert("Error", "Por favor, seleccione el tipo de sangre.", "OK");
            return;
        }

        // Verificar que la contrase�a no est� vac�a
        var password = passwordEntry.Text;
        if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor, ingrese una contrase�a.", "OK");
            return;
        }

        // Mapear los valores al DTO
        var dto = new UserDto
        {
            Name = nombre,
            Email = email,
            Dni = dni,
            Cellphone = celular,
            Gender = gender,
            Blood = blood,    // Asignar el valor del tipo de sangre como string
            Allergies = alergias,
            Password = password,
            Role = "1", // Rol por defecto
            Birthday = fechaNacimiento,
            Address = direccion,
            IsDonor = esDonante
        };

        // Llamar al servicio de registro
        var result = await _registroService.RegisterPacienteAsync(dto);

        // Mostrar el resultado al usuario
        await DisplayAlert("Resultado del Registro", result, "OK");      
            await Application.Current.MainPage.Navigation.PushAsync(new ClinicalUtp.MainPage());

    }

    // Funci�n para limpiar los campos
    private void ClearFields()
    {
        nombreEntry.Text = string.Empty;
        emailEntry.Text = string.Empty;
        dniEntry.Text = string.Empty;
        cellphoneEntry.Text = string.Empty;
        addressEntry.Text = string.Empty;
        allergiesEntry.Text = string.Empty;
        passwordEntry.Text = string.Empty;
        genderPicker.SelectedIndex = -1;
        bloodPicker.SelectedIndex = -1;
        isDonorSwitch.IsToggled = false;
        birthdayPicker.Date = DateTime.Now;
    }
}
