using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.viewModels
{
    public class referencesViewModel : INotifyPropertyChanged
    {
        private readonly AppointmentsServices _referenceService;
        public ObservableCollection<ReferencesDto> References { get; set; } = new();

        // Agregar variables para Reason y ReferenceDate
        public ObservableCollection<string> Reasons { get; set; } = new();
        public ObservableCollection<DateTime> ReferenceDates { get; set; } = new();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public referencesViewModel(AppointmentsServices referenceService, int doctorId)
        {
            _referenceService = referenceService;
            _referenceService = referenceService ?? throw new ArgumentNullException(nameof(referenceService));
            References = new ObservableCollection<ReferencesDto>();
            LoadReferencesAsync(doctorId).ConfigureAwait(false);
        }

        public async Task LoadReferencesAsync(int doctorId)
        {
            try
            {
                IsLoading = true;
                var references = await _referenceService.GetReferencesByDoctorIdAsync(doctorId);
                References.Clear();
                Reasons.Clear();
                ReferenceDates.Clear();

                // Al agregar las referencias, también se añaden los Reason y ReferenceDate
                foreach (var reference in references)
                {
                    References.Add(reference);
                    Reasons.Add(reference.Reason); // Añadimos la razón a la colección
                    ReferenceDates.Add(reference.ReferenceDate); // Añadimos la fecha a la colección
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
