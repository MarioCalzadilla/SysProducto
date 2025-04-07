using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCSysProducto.EN;
using MCSysProducto.BL;
using Rotativa.AspNetCore;
using OfficeOpenXml;

namespace MCSysProducto.WebApp.Controllers
{
    public class ClienteController : Controller
    {
        readonly ClienteBL _clienteBL;
        public ClienteController(ClienteBL pClienteBL) 
        {
            _clienteBL = pClienteBL;
        }

        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.CrearAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.ModificarAsync(pCliente);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = await _clienteBL.EliminararAsync(new Cliente { Id = id });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }

        public async Task<ActionResult> ReporteClientes()
        {
            var cliente = await _clienteBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpCliente", cliente);
        }

        public async Task<ActionResult> ReporteClientesExcel()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Clientes");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Dui";
                hojaExcel.Cells["C1"].Value = "Direccion";
                hojaExcel.Cells["D1"].Value = "Telefono";
                hojaExcel.Cells["E1"].Value = "Email";

                int row = 2;
                foreach (var cliente in clientes)
                {
                    hojaExcel.Cells[row, 1].Value = cliente.Nombre;
                    hojaExcel.Cells[row, 2].Value = cliente.Dui;
                    hojaExcel.Cells[row, 3].Value = cliente.Direccion;
                    hojaExcel.Cells[row, 4].Value = cliente.Telefono;
                    hojaExcel.Cells[row, 5].Value = cliente.Email;
                    row++;
                }
                hojaExcel.Cells["A:E"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openmxlformats-officedocument.spreadsheet.sheet", "ReporteClientesExcel.xlsx");
            }
        }

            public async Task<ActionResult> SubirExcelCliente(IFormFile archivoExcel)
            {
                if (archivoExcel == null || archivoExcel.Length == 0)
                {
                    return RedirectToAction("Index");
                }
                var clientes = new List<Cliente>();

                using (var stream = new MemoryStream())
                {
                    await archivoExcel.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var hojaExcel = package.Workbook.Worksheets[0];

                        int rowCount = hojaExcel.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var nombre = hojaExcel.Cells[row, 1].Text;
                            var Dui = hojaExcel.Cells[row, 2].Text;
                            var direccion = hojaExcel.Cells[row, 3].Text;
                            var telefono = hojaExcel.Cells[row, 4].Text;
                            var email = hojaExcel.Cells[row, 5].Text;



                            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(Dui) || string.IsNullOrEmpty(direccion))
                                continue;
                            clientes.Add(new Cliente
                            {
                                Nombre = nombre,
                                Dui = Dui,
                                Direccion = direccion,
                                Telefono = telefono,
                                Email = email
                            });
                        }
                    }

                    if (clientes.Count > 0)
                    {
                        await _clienteBL.AgregarTodosAsync(clientes);
                    }
                    return RedirectToAction("Index");
                }

            }
        }
}
