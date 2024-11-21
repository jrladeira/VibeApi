namespace VibeApi.Models;

public class ErroModel
{
    public bool Status = false;
    public List<string> Mensagens { get; set; } = [];
}
