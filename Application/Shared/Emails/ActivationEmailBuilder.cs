
using System.Text;
using System.Text.Encodings.Web;

namespace Application.Shared.Emails;

public static class ActivationEmailBuilder
{
    public static string Build(
        string userName,
        string appName,
        string activationUrl,
        int expiryHours,
        string message)
    {
        var sb = new StringBuilder();

        var safeName = HtmlEncoder.Default.Encode(userName);
        var safeApp = HtmlEncoder.Default.Encode(appName);
        var safeUrl = HtmlEncoder.Default.Encode(activationUrl);
        var safeMessage = HtmlEncoder.Default.Encode(message);

        const string orange = "#fd7e14";
        const string black = "#263238";

        sb.Append("<!DOCTYPE html>");
        sb.Append("<html lang='pt'>");
        sb.Append("<head><meta charset='UTF-8'>");
        sb.Append("<meta name='viewport' content='width=device-width,initial-scale=1.0'>");
        sb.Append("</head>");

        sb.Append("<body style='margin:0;background:#f4f4f4;");
        sb.Append("font-family:Arial,sans-serif;padding:20px;'>");

        sb.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
        sb.Append("<tr><td align='center'>");

        sb.Append("<table width='600' cellpadding='0' cellspacing='0' ");
        sb.Append("style='max-width:600px;width:100%;background:#fff;'>");

        // HEADER - LARANJA
        sb.Append("<tr><td bgcolor='" + orange + "' ");
        sb.Append("style='background-color:" + orange + ";padding:22px 25px;color:#fff;'>");

        sb.Append($"<h2 style='margin:0;font-size:22px;'>{safeApp}</h2>");
        sb.Append("<p style='margin:5px 0 0;font-size:12px;'>Portal do Cliente</p>");

        sb.Append("</td></tr>");

        // CONTEÚDO
        sb.Append("<tr><td style='padding:35px 30px;'>");

        sb.Append("<h2 style='color:#263238;font-size:21px;'>");
        sb.Append("Confirmação da conta");
        sb.Append("</h2>");

        sb.Append($"<p style='font-size:14px;color:#555;'>Olá {safeName},</p>");

        sb.Append("<p style='font-size:14px;color:#555;line-height:23px;'>");
        sb.Append(safeMessage);
        sb.Append("</p>");

        // BOTÃO - COMPATÍVEL COM ZIMBRA
        sb.Append("<table role='presentation' align='center' ");
        sb.Append("border='0' cellpadding='0' cellspacing='0' ");
        sb.Append("style='margin:30px auto;'>");

        sb.Append("<tr>");
        sb.Append("<td align='center' bgcolor='" + orange + "' ");
        sb.Append("style='background-color:" + orange + ";'>");

        sb.Append($"<a href='{safeUrl}' target='_blank' ");
        sb.Append("style='display:inline-block;padding:15px 30px;");
        sb.Append("font-family:Arial,sans-serif;font-size:14px;");
        sb.Append("font-weight:bold;color:#ffffff;text-decoration:none;");
        sb.Append("background-color:" + orange + ";'>");

        sb.Append("CONFIRMAR A MINHA CONTA");

        sb.Append("</a>");
        sb.Append("</td></tr></table>");

        // VALIDADE
        sb.Append("<p style='font-size:13px;color:#999;'>");
        sb.Append($"Este link expirará em {expiryHours} horas.");
        sb.Append("</p>");

        // LINK ALTERNATIVO
        sb.Append("<p style='font-size:12px;color:#888;'>");
        sb.Append("Caso o botão não funcione, copie o link e cole-o no navegador:");
        sb.Append("</p>");

        sb.Append($"<p style='font-size:11px;color:{orange};word-break:break-all;'>");
        sb.Append(safeUrl);
        sb.Append("</p>");

        sb.Append("</td></tr>");

        // FOOTER - PRETO
        sb.Append("<tr><td bgcolor='" + black + "' ");
        sb.Append("style='background-color:" + black + ";padding:20px;");
        sb.Append("text-align:center;color:#fff;'>");

        sb.Append("<p style='font-size:12px;margin:0 0 8px;'>");
        sb.Append($"© {DateTime.UtcNow.Year} {safeApp}. Todos os direitos reservados.");
        sb.Append("</p>");

        sb.Append("<p style='font-size:11px;color:#b0bec5;margin:0;'>");
        sb.Append("Este é um email automático. Por favor, não responda.");
        sb.Append("</p>");

        sb.Append("</td></tr>");

        sb.Append("</table></td></tr></table>");
        sb.Append("</body></html>");

        return sb.ToString();
    }
}
