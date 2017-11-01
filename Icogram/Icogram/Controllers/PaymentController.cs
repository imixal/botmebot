using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Icogram.Models.CompanyModels;
using Icogram.Models.Payments;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Icogram.ViewModels.Payments;
using Service;

namespace Icogram.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ICrudService<Payment> _paymentCrudService;
        private readonly IUserService _userService;
        private readonly ICrudService<PaymentType> _paymentTypeCrudService;
        private readonly IViewModelBuilder _viewModelBuilder;
        private readonly ICrudService<Company> _companyCrudService;


        public PaymentController(ICrudService<Payment> paymentCrudService, IUserService userService, ICrudService<PaymentType> paymentTypeCrudService, IViewModelBuilder viewModelBuilder, ICrudService<Company> companyCrudService)
        {
            _paymentCrudService = paymentCrudService;
            _userService = userService;
            _paymentTypeCrudService = paymentTypeCrudService;
            _viewModelBuilder = viewModelBuilder;
            _companyCrudService = companyCrudService;
        }


        public async Task<ActionResult> MyPayments()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<PaymentsPageViewModel>();
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            var payments = await _paymentCrudService.GetAllAsync();
            model.Payments = payments.Where(p => p.CompanyId == user.CompanyId).ToList();
            model.PaymentTypes = await _paymentTypeCrudService.GetAllAsync();

            return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> Index()
        {
            var model = await _viewModelBuilder.GetPageViewModelAsync<PaymentsPageViewModel>();
            model.Payments = await _paymentCrudService.GetAllAsync();
            model.PaymentTypes = await _paymentTypeCrudService.GetAllAsync();

            return View(model);
        }


        #region Command

        public async Task<ActionResult> CreatePaymentCommand(Payment payment)
        {
            var user = await _userService.GetByUserNameAsync(HttpContext.User.Identity.Name);
            payment.IsAproved = false;
            if (user.CompanyId != null)
            {
                payment.CompanyId = user.CompanyId.Value;
                payment.PaymentDate = DateTime.UtcNow;
                await _paymentCrudService.CreateAsync(payment);
            }

            return RedirectToAction("MyPayments");
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task AprovePaymentCommand(int id)
        {
            var payment = await _paymentCrudService.GetByIdAsync(id);
            payment.IsAproved = true;
            await _paymentCrudService.UpdateAsync(payment);
            var company = await _companyCrudService.GetByIdAsync(payment.CompanyId);
            company.End = payment.PaymentDate.AddMonths(payment.PaymentType.NumberOfMonth);
            company.Price += payment.PaymentType.Eth;
            await _companyCrudService.UpdateAsync(company);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeletePaymentCommand(int id)
        {
            var payment = await _paymentCrudService.GetByIdAsync(id);
            await _paymentCrudService.DeleteAsync(payment);
        }

        public async Task CancelPaymentCommand(int id)
        {
            var payment = await _paymentCrudService.GetByIdAsync(id);
            if (!payment.IsAproved)
            {
                await _paymentCrudService.DeleteAsync(payment);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task UpdatePaymentTypeCommand(PaymentType newPaymentType)
        {
            var paymentType = await _paymentTypeCrudService.GetByIdAsync(newPaymentType.Id);
            paymentType.Eth = newPaymentType.Eth;
            paymentType.NumberOfMonth = newPaymentType.NumberOfMonth;
            paymentType.Text = newPaymentType.Text;

            await _paymentTypeCrudService.UpdateAsync(paymentType);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task DeletePaymentTypeCommand(int id)
        {
            var paymentType = await _paymentTypeCrudService.GetByIdAsync(id);
            await _paymentTypeCrudService.DeleteAsync(paymentType);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task CreatePaymentTypeCommand(PaymentType paymentType)
        {
            await _paymentTypeCrudService.CreateAsync(paymentType);
        }

        #endregion
    }
}