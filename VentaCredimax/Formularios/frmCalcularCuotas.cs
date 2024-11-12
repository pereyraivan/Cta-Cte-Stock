using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VentaCredimax.Formularios
{
    public partial class frmCalcularCuotas : Form
    {
        public frmCalcularCuotas()
        {
            InitializeComponent();
        }

        private void frmCalcularCuotas_Load(object sender, EventArgs e)
        {

        }
        private void CalcularCuotas()
        {
            // Verificar que el campo Precio esté ingresado
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                errorProvider1.SetError(txtPrecio, "Campo obligatorio");
                return;
            }
            // Convertir el precio total
            decimal precioTotal = Convert.ToDecimal(txtPrecio.Text);

            // Caso 1: Si se ha ingresado el Precio y el Valor de la Cuota
            if (!string.IsNullOrEmpty(txtValorCuota.Text))
            {
                // Convertir el valor de la cuota
                decimal valorCuota = Convert.ToDecimal(txtValorCuota.Text);

                // Calcular el número de cuotas y el resto
                int cuotasEnteras = (int)(precioTotal / valorCuota);
                decimal resto = precioTotal % valorCuota;

                // Formatear los valores para mostrar
                string valorCuotaFormateado = valorCuota.ToString("N2");
                string primeraCuotaFormateada = (valorCuota + resto).ToString("N2");

                // Si hay un resto, sumarlo a la primera cuota
                if (resto > 0)
                {
                    lblMensaje.Text = $"Primera cuota de ${primeraCuotaFormateada} y {cuotasEnteras - 1} cuotas de ${valorCuotaFormateado}.";
                }
                else
                {
                    lblMensaje.Text = $"{cuotasEnteras} cuotas de ${valorCuotaFormateado}.";
                }

                // Mostrar el número de cuotas en el TextBox
                txtCuotas.Text = cuotasEnteras.ToString();
            }
            // Caso 2: Si se ha ingresado el Precio y el Número de Cuotas
            else if (!string.IsNullOrEmpty(txtCuotas.Text))
            {
                // Convertir el número de cuotas
                int numeroCuotas = Convert.ToInt32(txtCuotas.Text);

                // Calcular el valor de cada cuota
                decimal valorCuota = precioTotal / numeroCuotas;

                // Formatear el valor de cada cuota
                string valorCuotaFormateado = valorCuota.ToString("N2");

                // Mostrar el mensaje con el valor de cada cuota
                lblMensaje.Text = $"{numeroCuotas} cuotas de ${valorCuotaFormateado}.";

                // Mostrar el valor de la cuota en el TextBox de valor de cuota
                txtValorCuota.Text = valorCuotaFormateado;
            }
            else
            {
                // Si ninguno de los campos necesarios está completo, mostrar un error
                errorProvider1.SetError(txtValorCuota, "Debes ingresar el valor de la cuota o el número de cuotas.");
            }
        }
        private void btnCalcularCuotas_Click(object sender, EventArgs e)
        {
            if (txtPrecio.Text == "")
            {
                errorProvider1.SetError(txtPrecio, "Campo obligatorio");
            }
            else
            {
                CalcularCuotas();
            }
        }
        private void Limpiar()
        {
            txtPrecio.Text = "";
            txtCuotas.Text = "";
            txtValorCuota.Text = "";
            lblMensaje.Text = "";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
