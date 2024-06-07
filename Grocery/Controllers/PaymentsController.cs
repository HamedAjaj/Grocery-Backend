using Grocery.Domain.Entities;
using Grocery.Domain.IServices.IPaymentServices;
using Grocery.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Grocery.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private const string endpointSecret = "whsec_e18457f722baed4a65cab45ba48abdfaa7b78ae9f4d642ba4e76ef5df96ebcf5";
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{basketid}")] // Sigment not query string
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            return basket is null ? BadRequest(new ApiResponse(400, "Problem in Basket")) : Ok(basket);
        }



        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        await _paymentService.UpdatePaymentIntentToSucceededORFailed(paymentIntent.Id, true);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        await _paymentService.UpdatePaymentIntentToSucceededORFailed(paymentIntent.Id, false);
                        break;
                }


                //// Handle the event
                //if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                //{
                //}
                //else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                //{
                //}
                //// ... handle other event types
                //else
                //{
                //    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                //}

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
