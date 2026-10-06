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
            try { this.dgvProd.CellClick += new DataGridViewCellEventHandler(this.dgvProd_CellClick); } catch { }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            
            cargarProductos(txtBusq.Text.Trim());
        }

        private void cargarProductos(string filtro = "")
        {
            try { if (dgvProd.DataSource != null) dgvProd.DataSource = null; } catch { }
            try { EnsureColumns(); } catch { }

            dgvProd.Rows.Clear();
            dgvProd.Refresh();

            listaProductos = conexionDB.GetProductos(filtro);
            try
            {
                foreach (var pDbg in listaProductos)
                {
                    Console.WriteLine($"GetProductos: id={pDbg.ID} cantidad={pDbg.Cantidad} fecha_creacion={pDbg.FechaCreacion.ToString() ?? "NULL"} fecha_modificacion={pDbg.FechaModificacion.ToString() ?? "NULL"}");
                }
            }
            catch { }

            var imgCol = dgvProd.Columns["cImg"] as DataGridViewImageColumn;

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

                Image imgCell = Imagenes.AdjustImageForCell(img, 120, 300);

                int rowIndex = dgvProd.Rows.Add();
                var row = dgvProd.Rows[rowIndex];
                if (dgvProd.Columns["cID"] != null) row.Cells["cID"].Value = prod.ID;
                if (dgvProd.Columns["cNombre"] != null) row.Cells["cNombre"].Value = prod.Nombre;
                if (dgvProd.Columns["cPrecio"] != null) row.Cells["cPrecio"].Value = prod.Precio;
                if (dgvProd.Columns["cCantidad"] != null) row.Cells["cCantidad"].Value = prod.Cantidad;
                if (dgvProd.Columns["cImg"] != null) row.Cells["cImg"].Value = imgCell;
                if (dgvProd.Columns["cFechaCreacion"] != null) row.Cells["cFechaCreacion"].Value = prod.FechaCreacion.HasValue ? prod.FechaCreacion.Value.ToString("g") : string.Empty;
                if (dgvProd.Columns["cFechaModificacion"] != null) row.Cells["cFechaModificacion"].Value = prod.FechaModificacion.HasValue ? prod.FechaModificacion.Value.ToString("g") : string.Empty;

                if (imgCell != null)
                {
                    row.Height = imgCell.Height + 8;
                    try
                    {
                        if (imgCol != null && imgCell.Width + 8 > imgCol.Width)
                        {
                            imgCol.Width = imgCell.Width + 8;
                        }
                    }
                    catch { }
                }
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
            EnsureColumns();

            dgvProd.RowTemplate.Height = 100;
            var imgCol = dgvProd.Columns["cImg"] as DataGridViewImageColumn;
            if (imgCol != null)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Normal;
                imgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                imgCol.Width = 120;
            }

            cargarProductos();

        }

        private void EnsureColumns()
        {
            try
            {
                var headerToName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "id", "cID" }, { "identificador", "cID" },
                    { "nombre", "cNombre" }, { "name", "cNombre" },
                    { "precio", "cPrecio" }, { "price", "cPrecio" },
                    { "cantidad", "cCantidad" }, { "stock", "cCantidad" },
                    { "imagen", "cImg" }, { "imagen producto", "cImg" }, { "image", "cImg" },
                    { "creado", "cFechaCreacion" }, { "fecha_creacion", "cFechaCreacion" }, { "fecha creación", "cFechaCreacion" },
                    { "modificado", "cFechaModificacion" }, { "fecha_modificacion", "cFechaModificacion" }, { "modificación", "cFechaModificacion" }
                };

                foreach (DataGridViewColumn col in dgvProd.Columns)
                {
                    if (col == null) continue;
                    var header = (col.HeaderText ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(col.Name) || col.Name.StartsWith("Column", StringComparison.OrdinalIgnoreCase) || !headerToName.ContainsValue(col.Name))
                    {
                        if (headerToName.TryGetValue(header, out var expectedName))
                        {
                            if (!dgvProd.Columns.Contains(expectedName))
                            {
                                col.Name = expectedName;
                            }
                        }
                    }
                }
            }
            catch { }

            void ConfigureText(string name, string header, int width = 80, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
            {
                var c = dgvProd.Columns[name] as DataGridViewTextBoxColumn;
                if (c != null)
                {
                    c.HeaderText = header;
                    c.Width = width;
                    c.ValueType = typeof(string);
                    c.DefaultCellStyle.Alignment = align;
                    return;
                }
                dgvProd.Columns.Add(new DataGridViewTextBoxColumn() { Name = name, HeaderText = header, Width = width });
            }

            void ConfigureImage(string name, string header, int width = 120)
            {
                var c = dgvProd.Columns[name] as DataGridViewImageColumn;
                if (c != null)
                {
                    c.HeaderText = header;
                    c.Width = width;
                    c.ImageLayout = DataGridViewImageCellLayout.Normal;
                    return;
                }
                var ic = new DataGridViewImageColumn() { Name = name, HeaderText = header, Width = width, ImageLayout = DataGridViewImageCellLayout.Normal };
                dgvProd.Columns.Add(ic);
            }

            ConfigureText("cID", "ID", 50, DataGridViewContentAlignment.MiddleCenter);
            ConfigureText("cNombre", "Nombre", 200, DataGridViewContentAlignment.MiddleLeft);
            ConfigureText("cPrecio", "Precio", 80, DataGridViewContentAlignment.MiddleRight);
            ConfigureText("cCantidad", "Cantidad", 70, DataGridViewContentAlignment.MiddleCenter);
            ConfigureImage("cImg", "Imagen", 120);
            ConfigureText("cFechaCreacion", "Creado", 140, DataGridViewContentAlignment.MiddleCenter);
            ConfigureText("cFechaModificacion", "Modificado", 140, DataGridViewContentAlignment.MiddleCenter);

            try
            {
                string[] order = new string[] { "cID", "cNombre", "cPrecio", "cCantidad", "cImg", "cFechaCreacion", "cFechaModificacion" };
                int idx = 0;
                foreach (var n in order)
                {
                    if (dgvProd.Columns.Contains(n)) dgvProd.Columns[n].DisplayIndex = idx++;
                }
            }
            catch { }
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
            myProducto["imagen"] = Imagenes.ImageToByteArray(pbImagen.Image);
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

        private void dgvProd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                var row = dgvProd.Rows[e.RowIndex];
                if (dgvProd.Columns["cImg"] != null)
                {
                    var val = row.Cells["cImg"].Value;
                    if (val is Image)
                    {
                        pbImagen.Image = (Image)val;
                    }
                    else
                    {
                        try
                        {
                            if (row.Cells["cID"]?.Value != null)
                            {
                                int id = Convert.ToInt32(row.Cells["cID"].Value);
                                var p = listaProductos.FirstOrDefault(x => x.ID == id);
                                if (p != null && p.Imagen != null && p.Imagen.Length > 0)
                                {
                                    using (var ms = new System.IO.MemoryStream(p.Imagen))
                                    {
                                        pbImagen.Image = Image.FromStream(ms);
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }

                try { if (dgvProd.Columns["cID"] != null) txtID.Text = row.Cells["cID"].Value?.ToString() ?? ""; } catch { }
            }
            catch { }
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

