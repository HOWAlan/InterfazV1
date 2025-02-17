using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Panel_V2
{
    public partial class Form1 : Form
    {
        private double medAcot1 = 0;
        private double medCarrl1 = 0;
        private double medCarrl2 = 0;
        private double medCarrl3 = 0;
        private double medAcot2 = 0;

        // Diccionario que almacena qué TextBox deben mostrarse según la imagen seleccionada
        private Dictionary<string, List<TextBox>> configuracionCampos;

        public Form1()
        {
            InitializeComponent();
            InicializarDiccionario();
            HabilitarCampos(false);
            AsignarEventos(); // Llama al método para asignar eventos a los PictureBox
        }

        // Método que se ejecutará cuando hagas clic en un PictureBox de pnlImages
        private void Imagen_Click(object sender, EventArgs e)
        {
            PictureBox imagenSeleccionada = sender as PictureBox;

            if (imagenSeleccionada != null)
            {
                imgSelecc.Image = imagenSeleccionada.Image;

                MessageBox.Show("Imagen seleccionada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ocultar todos los TextBox antes de mostrar los necesarios
                HabilitarCampos(false);

                // Verificar si la imagen seleccionada está en el diccionario
                if (configuracionCampos.ContainsKey(imagenSeleccionada.Name))
                {
                    foreach (TextBox campo in configuracionCampos[imagenSeleccionada.Name])
                    {
                        campo.Enabled = true;
                        campo.Visible = true; // Mostrar los campos correspondientes
                    }
                }
            }
        }

        // Método que asigna dinámicamente el evento Click a los PictureBox dentro de pnlImages
        private void AsignarEventos()
        {
            // Recorre todos los controles dentro de pnlImages
            foreach (Control control in pnlImages.Controls)
            {
                // Verifica si el control es un PictureBox
                if (control is PictureBox pictureBox)
                {
                    // Asigna el evento Click al método Imagen_Click
                    pictureBox.Click += Imagen_Click;
                }
            }
        }

        private bool ValidarCampos()
        {
            try
            {
                // Lista de los TextBox que contienen valores numéricos
                TextBox[] campos = { acot1, carrl1, carrl2, carrl3, acot2 };

                foreach (TextBox campo in campos)
                {
                    // Si el campo está visible y vacío, mostrar advertencia y no permitir guardar
                    if (campo.Visible && string.IsNullOrWhiteSpace(campo.Text))
                    {
                        MessageBox.Show($"Es necesario llenar todos los campos antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // Convertir los valores ingresados a double y almacenarlos en las variables
                if (string.IsNullOrWhiteSpace(acot1.Text))
                {
                    medAcot1 = 0.3;
                }
                else
                {
                    medAcot1 = Convert.ToDouble(acot1.Text);
                }

                if (string.IsNullOrWhiteSpace(carrl1.Text))
                {
                    medCarrl1 = 0.3;
                }
                else
                {
                    medCarrl1 = Convert.ToDouble(carrl1.Text);
                }

                if (string.IsNullOrWhiteSpace(carrl2.Text))
                {
                    medCarrl2 = 0.3;
                }
                else
                {
                    medCarrl2 = Convert.ToDouble(carrl2.Text);
                }

                if (string.IsNullOrWhiteSpace(carrl3.Text))
                {
                    medCarrl3 = 0.3;
                }
                else
                {
                    medCarrl3 = Convert.ToDouble(carrl3.Text);
                }

                if (string.IsNullOrWhiteSpace(acot2.Text))
                {
                    medAcot2 = 0.3;
                }
                else
                {
                    medAcot2 = Convert.ToDouble(acot2.Text);
                }
            }
            catch
            {
                MessageBox.Show("Por favor, ingresa valores numéricos en los campos correspondientes.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado una imagen
            if (imgSelecc.Image == null)
            {
                MessageBox.Show("Debes seleccionar una imagen antes de ingresar los datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llamamos al método de validación antes de continuar
            if (!ValidarCampos())
            {
                return; // Si la validación falla, no continúa
            }

            // Construcción del resumen de datos ingresados
            string resumen = "Resumen de datos ingresados:\n";
            resumen += $"Fecha: {date.Text}\n";
            resumen += $"Carretera: {highRoad.Text}\n";
            resumen += $"Kilómetro: {km.Text} metro(s)\n";
            resumen += $"Tipo de cuerpo: {type.Text}\n";

            // Solo agregar los valores de los TextBox que estén visibles con la unidad "metro(s)"
            if (acot1.Visible) resumen += $"Acotamiento 1: {medAcot1} metro(s)\n";
            if (carrl1.Visible) resumen += $"Carril 1: {medCarrl1} metro(s)\n";
            if (carrl2.Visible) resumen += $"Carril 2: {medCarrl2} metro(s)\n";
            if (carrl3.Visible) resumen += $"Carril 3: {medCarrl3} metro(s)\n";
            if (acot2.Visible) resumen += $"Acotamiento 2: {medAcot2} metro(s)\n";

            // Crear un formulario nuevo para mostrar la imagen con los datos
            Form resumenForm = new Form
            {
                Text = "Resumen",
                Size = new Size(500, 600), //Tamaño de la ventana
                StartPosition = FormStartPosition.CenterScreen
            };

            //Label para los datos
            Label lblResumen = new Label
            {
                Text = resumen,
                AutoSize = false,
                Size = new Size(450, 200),
                Location = new Point(20, 20)
            };

            //PictureBox para la imagen seleccionada
            PictureBox picResumen = new PictureBox
            {
                Image = imgSelecc.Image,
                SizeMode = PictureBoxSizeMode.Zoom, // Cambiar a Zoom para evitar deformación
                Size = new Size(300, 250), // Mantener el tamaño grande
                Location = new Point(100, 230),
                
            };

            //Botón para cerrar el formulario
            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(100, 40),
                Location = new Point(200, 500)
            };
            btnCerrar.Click += (s, ev) => { resumenForm.Close(); };

            // Agregar los controles al formulario
            resumenForm.Controls.Add(lblResumen);
            resumenForm.Controls.Add(picResumen);
            resumenForm.Controls.Add(btnCerrar);

            // Mostrar el formulario
            resumenForm.ShowDialog();
        }

        private void HabilitarCampos(bool estado)
        {
            // Lista de todos los TextBox a controlar
            TextBox[] campos = { acot1, carrl1, carrl2, carrl3, acot2 };

            foreach (TextBox campo in campos)
            {
                campo.Enabled = estado;
                campo.Visible = estado;

                if (!estado) // Si el campo se oculta, lo dejamos vacío visualmente
                {
                    campo.Text = "";
                }
            }
        }

        private void InicializarDiccionario()
        {
            configuracionCampos = new Dictionary<string, List<TextBox>>
            {
                { "opc3Carr2Acot", new List<TextBox> { acot2, carrl3, carrl2, carrl1, acot1 } },
                { "opc3Carr1Acot", new List<TextBox> { carrl3, carrl2, carrl1, acot1 } },
                { "opc3Carr", new List<TextBox> { carrl3, carrl2, carrl1 } },
                { "opc2Carr2Acot", new List<TextBox> { acot2, carrl2, carrl1, acot1 } },
                { "opc2Carr1Acot", new List<TextBox> { carrl2, carrl1, acot1 } },
                { "opc2Carr", new List<TextBox> { carrl2, carrl1 } },
                { "opc1Carr2Acot", new List<TextBox> { acot2, carrl1, acot1 } },
                { "opc1Carr1Acot", new List<TextBox> { carrl1, acot1 } },
                { "opc1Carr", new List<TextBox> { carrl1 } }
            };
        }
    }
}
