using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using System.Threading.Tasks;
using UnityEngine;


public class Purchase_Manager : MonoBehaviour
{
    private Payment _payment;
    [SerializeField] private string Key = "";


    public async Task<bool> Init()
    {
        SecurityCheck securityCheck = SecurityCheck.Enable(Key);
        PaymentConfiguration paymentConfiguration = new PaymentConfiguration(securityCheck);
        _payment = new Payment(paymentConfiguration);
        
        var result = await _payment.Connect();
        return result.status == Status.Success;
    }
    
    public async Task<Result<PurchaseInfo>> Purchase(string PrId)
    {
        var result = await _payment.Purchase(PrId);

        return result;
    }
    public async Task<Result<bool>> Consume(string PuTo)
    {
        var result = await _payment.Consume(PuTo);

        return result;
    }
    private void OnApplicationQuit()
    {
        _payment.Disconnect();
    }

}
