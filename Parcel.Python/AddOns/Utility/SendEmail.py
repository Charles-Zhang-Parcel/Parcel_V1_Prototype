import smtplib # Import smtplib for the actual sending function
from email.mime.text import MIMEText # Import the email modules we'll need

# Might want to skip this and directly use reflection by extracting MethodInfo
def NodeExport():
    return NodeDefinition([],[], lambda inputs: SendEmail()) # PENDING...

def SendEmail(sender, receiver, title, message, server='localhost'):  
    msg = MIMEText()    
    msg['Subject'] = message
    msg['From'] = sender
    msg['To'] = receiver
    
    s = smtplib.SMTP(server)
    s.sendmail(sender, [receiver], msg.as_string())
    s.quit()