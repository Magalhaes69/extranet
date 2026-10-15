using System.Net;

namespace Persistence.SAP_Helps
{
    public static class EmailTemplates
    {
        private const string Brand = "#0d6efd";
        private const string Text = "#344054";
        private const string Muted = "#667085";

        public static string Otp(string code, string appName, int expiryMinutes)
        {
            var content = $@"
            <h1 style='margin:0 0 16px;font-size:22px;color:{Text};'>Verificação de segurança</h1>
            <p style='margin:0 0 16px;font-size:15px;line-height:24px;color:{Text};'>
                Recebemos um pedido de acesso à sua conta. Utilize o código abaixo para concluir o login:
            </p>
            <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='margin:24px 0;'>
                <tr>
                    <td align='center'>
                        <div style='display:inline-block;padding:16px 28px;background:#f2f4f7;border:1px dashed #d0d5dd;border-radius:10px;
                                    font-family:Consolas,Menlo,monospace;font-size:34px;font-weight:bold;letter-spacing:8px;color:{Text};'>
                            {WebUtility.HtmlEncode(code)}
                        </div>
                    </td>
                </tr>
            </table>
            <p style='margin:0 0 8px;font-size:14px;color:{Text};'>
                O código expira em <strong>{expiryMinutes} minutos</strong>.
            </p>
            <p style='margin:0;font-size:13px;line-height:20px;color:{Muted};'>
                Se não fez este pedido, ignore este e-mail. Nunca partilhe este código com ninguém,
                nem mesmo com a nossa equipa.
            </p>";

            return Layout(appName, $"O seu código de verificação é {code}", content);
        }

        public static string Invitation(string? firstName, string link, string appName, int validDays)
        {
            var greeting = string.IsNullOrWhiteSpace(firstName)
                ? "Olá,"
                : $"Olá {WebUtility.HtmlEncode(firstName)},";
            var safeLink = WebUtility.HtmlEncode(link);

            var content = $@"
            <h1 style='margin:0 0 16px;font-size:22px;color:{Text};'>Bem-vindo à {WebUtility.HtmlEncode(appName)}</h1>
            <p style='margin:0 0 12px;font-size:15px;line-height:24px;color:{Text};'>{greeting}</p>
            <p style='margin:0 0 16px;font-size:15px;line-height:24px;color:{Text};'>
                Foi criada uma conta para si na {WebUtility.HtmlEncode(appName)}. Aqui pode consultar as suas encomendas,
                faturas e pagamentos a qualquer momento.
            </p>
            <p style='margin:0 0 24px;font-size:15px;line-height:24px;color:{Text};'>
                Para começar, defina a sua palavra-passe:
            </p>
            <table role='presentation' cellpadding='0' cellspacing='0' style='margin:0 auto 24px;'>
                <tr>
                    <td align='center' bgcolor='{Brand}' style='border-radius:8px;'>
                        <a href='{safeLink}' target='_blank'
                           style='display:inline-block;padding:14px 32px;font-size:15px;font-weight:bold;color:#ffffff;
                                  text-decoration:none;border-radius:8px;'>
                            Definir palavra-passe
                        </a>
                    </td>
                </tr>
            </table>
            <p style='margin:0 0 8px;font-size:13px;line-height:20px;color:{Muted};'>
                Este link é válido durante <strong>{validDays} dias</strong>. Se o botão não funcionar, copie e cole
                este endereço no seu navegador:
            </p>
            <p style='margin:0 0 16px;font-size:12px;line-height:18px;word-break:break-all;'>
                <a href='{safeLink}' style='color:{Brand};'>{safeLink}</a>
            </p>
            <p style='margin:0;font-size:13px;line-height:20px;color:{Muted};'>
                Se não esperava este e-mail, pode ignorá-lo.
            </p>";

            return Layout(appName, "A sua conta foi criada. Defina a sua palavra-passe para começar.", content);
        }

        private static string Layout(string appName, string preheader, string content)
        {
            var name = WebUtility.HtmlEncode(appName);

            return $@"<!DOCTYPE html>
<html lang='pt'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{name}</title>
</head>
<body style='margin:0;padding:0;background:#f4f6f9;font-family:Segoe UI,Arial,sans-serif;'>
    <div style='display:none;max-height:0;overflow:hidden;opacity:0;'>{WebUtility.HtmlEncode(preheader)}</div>
    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background:#f4f6f9;padding:32px 12px;'>
        <tr>
            <td align='center'>
                <table role='presentation' width='100%' cellpadding='0' cellspacing='0'
                       style='max-width:560px;background:#ffffff;border-radius:12px;overflow:hidden;
                              box-shadow:0 2px 8px rgba(16,24,40,0.06);'>
                    <tr>
                        <td style='background:{Brand};padding:20px 32px;'>
                            <span style='font-size:20px;font-weight:bold;color:#ffffff;'>{name}</span>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:32px;'>
                            {content}
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:20px 32px;background:#f9fafb;border-top:1px solid #eaecf0;'>
                            <p style='margin:0;font-size:12px;line-height:18px;color:{Muted};'>
                                Este é um e-mail automático, por favor não responda.<br>
                                © {DateTime.UtcNow.Year} {name}. Todos os direitos reservados.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }
    }
}
