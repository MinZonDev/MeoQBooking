<?php

/**
 * Once you've completed Steps 1-3 below, upload this file to the "quform"
 * folder on your web server and visit the page e.g.
 * http://www.example.com/quform/smtp-check.php
 */

error_reporting(-1);
@ini_set('display_errors', 'On');
require_once dirname(__FILE__) . '/lib/PHPMailerAutoload.php';
echo '<pre>';

/*
 * Step 1 - Enter the recipient email address
 */
$to = 'me@example.com';

/*
 * Step 2 - Enter a "From" email address
 */
$from = 'from@example.com';

/*
 * Step 3 - Enter the SMTP server settings
 *
 * host - SMTP server (e.g. smtp.example.com)
 * port - SMTP port (e.g. 25)
 * username - SMTP username
 * password - SMTP password
 * encryption - SMTP encryption (e.g. ssl or tls)
 */
$config['smtp'] = array(
    'host' => '',
    'port' => 25,
    'username' => '',
    'password' => '',
    'encryption' => ''
);

if (!empty($to) && $to != 'me@example.com') {
    $mailer = new PHPMailer(true);

    $mailer->isSMTP();
    $mailer->SMTPDebug = 3;
    $mailer->Host = $config['smtp']['host'];
    $mailer->Port = $config['smtp']['port'];
    if ($config['smtp']['username']) {
        $mailer->SMTPAuth = true;
    }
    $mailer->Username = $config['smtp']['username'];
    $mailer->Password = $config['smtp']['password'];
    $mailer->SMTPSecure = $config['smtp']['encryption'];

    $mailer->Subject = 'Test message from Quform';
    $mailer->Body = 'Your server can send email successfully :)';

    $mailer->addAddress($to);
    $mailer->setFrom($from);

    if ($mailer->send()) {
        echo 'Email sent, check your inbox.';
    } else {
        echo 'Mailer error: ' . $mailer->ErrorInfo;
    }
} else {
    echo 'Test email not sent - no recipient set.';
}

echo '</pre>';