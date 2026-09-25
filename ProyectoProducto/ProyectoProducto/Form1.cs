using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoProducto
{
    public partial class Form1 : Form
    {
        private List<Productos> listaProductos;
        private Dictionary<string, object> myProducto = new Dictionary<string, object>();

        public Form1()
        {
            InitializeComponent();
            listaProductos = new List<Productos>();
            this.txtBusq.TextChanged += new EventHandler(this.txtBusqueda_TextChanged);
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            cargarProductos(txtBusq.Text.Trim());
        }

        private void cargarProductos(string filtro = "")
        {
            dgvProd.Rows.Clear();
            dgvProd.Refresh();
            listaProductos = conexionDB.GetProductos(filtro);
            foreach (var prod in listaProductos)
            {
                Image img = null;
                if (prod.Imagen != null && prod.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(prod.Imagen))
                    {
                        using (Bitmap bmp = new Bitmap(ms))
                        {
                            img = new Bitmap(bmp); 
                        }
                    }
                }
                dgvProd.Rows.Add(prod.ID, prod.Nombre, prod.Precio, prod.Cantidad, img);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!datosCorrectos())
            {
                return;
            }

            CargarDatosProductos();

            if (conexionDB.InsertSeguro("productos", myProducto))
            {
                MessageBox.Show("Se ha guardado satisfactoriamente el registro");
                cargarProductos(); 
            }
            else
            {
                MessageBox.Show("Error al guardar el registro");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvProd.RowTemplate.Height = 100;
            var imgCol = dgvProd.Columns["cImg"] as DataGridViewImageColumn;
            if(imgCol != null)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                imgCol.Width = 120;
            }

            cargarProductos();

        }
        private void CargarDatosProductos()
        {
            if (myProducto == null) myProducto = new Dictionary<string, object>();
            myProducto.Clear();

            int cantidad = 0;
            decimal precio = 0m;

            int.TryParse(txtCant.Text.Trim(), out cantidad);
            decimal.TryParse(txtPrec.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out precio);

            myProducto["cantidad"] = cantidad;
            myProducto["precio"] = precio;
            myProducto["nombre"] = txtNom.Text.Trim();

            myProducto["imagen"] = ImageToByteArray(pbImagen.Image);
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (var mMemoryStream = new MemoryStream())
            {
                image.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return mMemoryStream.ToArray();
            }
        }

        private bool datosCorrectos()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Ingrese el Nombre del Producto");
                txtNom.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrec.Text))
            {
                MessageBox.Show("Ingrese el Precio");
                txtPrec.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCant.Text))
            {
                MessageBox.Show("Ingrese la Cantidad");
                txtCant.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrec.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal precio))
            {
                MessageBox.Show("Ingrese un Precio correcto");
                txtPrec.Focus();
                return false;
            }

            if (!int.TryParse(txtCant.Text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out int cantidad))
            {
                MessageBox.Show("Ingrese una cantidad correcta");
                txtCant.Focus();
                return false;
            }

            return true;
        }

        private void txtBusq_TextChanged(object sender, EventArgs e)
        {
            cargarProductos(txtBusq.Text.Trim());
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private byte[] imagenBytes;
        private void pbImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                pbImagen.Image = Image.FromFile(abrir.FileName);

                imagenBytes = File.ReadAllBytes(abrir.FileName);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(dgvProd.SelectedRows.Count > 0)
            {
                int idProducto = Convert.ToInt32(dgvProd.SelectedRows[0].Cells["cID"].Value);
                if (conexionDB.EliminarProducto(idProducto))
                {
                    MessageBox.Show("Producto eliminado");
                    cargarProductos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNom.Text = "";
            txtPrec.Text = "";
            txtCant.Text = "";
            txtID.Text = "";
            pbImagen.Image = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtID.Text.Trim(), out int idToUpdate) && idToUpdate > 0 && datosCorrectos())
            {
                CargarDatosProductos();
                if (conexionDB.UpdateProducto(idToUpdate, myProducto))
                {
                    MessageBox.Show("Registro actualizado.");
                    cargarProductos(txtBusq.Text.Trim());
                }
                else
                {
                    MessageBox.Show("Error al actualizar el registro.");
                }
                return;
            }

            if (dgvProd.SelectedRows.Count > 0)
            {
                int idProducto = Convert.ToInt32(dgvProd.SelectedRows[0].Cells["cID"].Value);
                Productos producto = listaProductos.FirstOrDefault(p => p.ID == idProducto);
                if (producto != null)
                {
                    txtID.Text = producto.ID.ToString();
                    txtNom.Text = producto.Nombre;
                    txtPrec.Text = producto.Precio.ToString();
                    txtCant.Text = producto.Cantidad.ToString();
                    if (producto.Imagen != null && producto.Imagen.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(producto.Imagen))
                        {
                            pbImagen.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pbImagen.Image = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto");
            }
        }
    }
}

