using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
using Microsoft.Maui.Controls;

namespace ClinicalUtp.ViewModels
{
    public class DonacionViewModel : BindableObject
    {
        private readonly PacienteServices _pacienteService;

        // ObservableCollection para la lista de pacientes
        public ObservableCollection<UserDto> Pacientes { get; private set; }

        // Propiedad para el paciente seleccionado
        private UserDto _selectedDonante;
        public UserDto SelectedDonante
        {
            get => _selectedDonante;
            set
            {
                if (_selectedDonante != value)
                {
                    _selectedDonante = value;
                    OnPropertyChanged(); // Notifica cambio
                    UserId = _selectedDonante?.Id ?? 0; // Actualiza UserId
                }
            }
        }

        // Propiedad para el UserId seleccionado
        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged(); // Notifica cambio
                }
            }
        }

        // Comandos
        public ICommand LoadPacientesCommand { get; }
        public ICommand AddDonationCommand { get; }

        public DonacionViewModel(PacienteServices pacienteService)
        {
            _pacienteService = pacienteService ?? throw new ArgumentNullException(nameof(pacienteService));
            Pacientes = new ObservableCollection<UserDto>();

            // Inicializa comandos
            LoadPacientesCommand = new Command(async () => await LoadPacientesAsync());
            AddDonationCommand = new Command<DonationDto>(async (donation) => await AddDonationAsync(donation));

            // Cargar pacientes al iniciar
            LoadPacientesCommand.Execute(null);
        }

        // Método para cargar pacientes
        private async Task LoadPacientesAsync()
        {
            try
            {
                var pacientes = await _pacienteService.GetAllUserDetailsAsync();

                // Actualiza la lista de pacientes
                Pacientes.Clear();
                foreach (var paciente in pacientes)
                {
                    Pacientes.Add(paciente);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar los pacientes: {ex.Message}");
            }
        }

        // Método para agregar una donación
        public async Task AddDonationAsync(DonationDto donationDto)
        {
            try
            {
                if (SelectedDonante == null)
                {
                    Console.WriteLine("Debe seleccionar un paciente.");
                    return;
                }

                // Crear el objeto DonationDto con la fecha actual
                donationDto.UserId = UserId;
                donationDto.CreatedAt = DateTime.Now;

                // Llamar al servicio para agregar la donación
                var donation = await _pacienteService.AddDonationAsync(donationDto);

                // Lógica después de agregar la donación, como notificar al usuario
                Console.WriteLine($"Donación registrada con éxito para el usuario ID: {donation.UserId}.");
            }
            catch (Exception ex)
            {
                // Manejar el error
                Console.WriteLine($"Error al agregar la donación: {ex.Message}");
            }
        }
    }
}