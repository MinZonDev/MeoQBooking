<?php

/**
 * Enable debug mode. Quform will try to display any fatal PHP errors or exceptions at
 * your form. It's useful to have this enabled while developing your form, but
 * you should set this to false on production sites.
 */
define('QUFORM_DEBUG', true);

/** DO NOT CHANGE THESE 2 LINES **/
define('QUFORM_ROOT', realpath(dirname(__FILE__)));
require_once QUFORM_ROOT . '/common.php';
/** DO NOT CHANGE THESE 2 LINES **/

/** FORM SETTINGS **/

/**
 * Success message, displayed when the form is successfully submitted
 */
$config['successMessage'] = 'Your message has been sent, thank you.';

/**
 * Whether or not to send the notification email. You may wish to disable this if you are
 * saving the form data to the database for example. true or false
 */
$config['email'] = true;

/**
 * Configure the recipients of the notification email message.  For a single
 * recipient use the following format:
 *
 * $config['recipients'] = 'me@example.com';
 *
 * You can add multiple email addresses by adding one on each line inside an
 * array, enclosed in quotes, separated by commas. E.g.
 *
 * $config['recipients'] = array(
 *     'recipient1@example.com',
 *     'recipient2@example.com'
 * );
 */
$config['recipients'] = '';

/**
 * Set the "From" address of the emails. You should set this to the contact
 * email address of your website. Some hosts require that the email
 * address is one that is associated with your hosting account.
 *
 * You can set this to be an email address string e.g.
 *
 * $config['from'] = 'company@example.com';
 *
 * Or you can also include a name with your email address using an array e.g.
 *
 * $config['from'] = array('company@example.com' => 'Company');
 *
 * Or you can set it to use a submitted email address. This example will get the
 * submitted address in the 'email' field. E.g.
 *
 * $config['from'] = '%email%';
 */
$config['from'] = '';

/**
 * The subject of the notification email message. %first_name% will be replaced
 * with the form submitted value in the first_name field.
 */
$config['subject'] = 'Validators example';

/**
 * Set the "Reply-To" email address of the notification email to
 * the email address submitted in the email field.
 */
$config['replyTo'] = '%email%';

/**
 * The file containing the HTML body of the notification email.
 */
$config['emailBody'] = '/emails/notification.php';

/**
 * Whether or not to show empty fields from the notification email. true or false
 */
$config['showEmptyFields'] = false;

/**
 * Whether or not to send an autoreply email. true or false
 */
$config['autoreply'] = false;

/**
 * Sets the autoreply recipient to the email address submitted to the email field
 *
 * If you want the email to show from their name submitted from the name field as well, you
 * can use the code:
 *
 * $config['autoreplyRecipient'] = array('%email%' => '%name%');
 */
$config['autoreplyRecipient'] = '%email%';

/**
 * The subject of the autoreply email
 */
$config['autoreplySubject'] = 'Thanks for your message, %name%';

/**
 * Set the "From" address of the autoreply email.
 * See the comment at the $config['from'] setting for options.
 */
$config['autoreplyFrom'] = '';

/**
 * The file containing the HTML body of the autoreply email
 */
$config['autoreplyBody'] = '/emails/autoreply.php';

/**
 * Redirect the user when the form is successfully submitted by entering a URL here.
 * By default, users are not redirected.
 *
 * $config['redirect'] = 'http://www.example.com/thanks.html';
 */
$config['redirect'] = '';

/**
 * Whether or not to save the form data to a database. true or false
 *
 * You can configure the database settings further down in this file. See the documentation
 * for help.
 */
$config['database'] = false;

/**
 * Whether or not to save uploaded files to the server. true or false
 */
$config['saveUploads'] = false;

/**
 * The path to save any uploaded files. This folder must be writeable by the
 * web server, you may need to set the folder permissions to 777 on Linux servers.
 */
$config['uploadPath'] = QUFORM_ROOT . '/uploads';

/**
 * Set this to the URL of the above folder to be sent links to uploaded
 * files in the notification email. E.g.
 *
 * $config['uploadUrl'] = 'http://www.example.com/quform/uploads';
 */
$config['uploadUrl'] = '';

/**
 * (Optional) Configure your SMTP settings, only 'host' is required. If your server
 * needs authentication, set your username and password. If these settings are left
 * blank the emails will be sent using the PHP mail() function.
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

// Add the visitor IP to the email
$config['extra']['IP address'] = Quform::getIPAddress();

/** END FORM SETTINGS **/

/** FORM ELEMENT CONFIGURATION **/

/**
 * Configure the between 3 and 6 validated field
 * Filters: Trim
 * Validators: StringLength min 3 max 6, Required
 */
$between3and6 = new Quform_Element('between_three_six');
$between3and6->addValidator('required');
$between3and6->addValidator('stringLength', array('min' => 3, 'max' => 6));
$between3and6->addFilters(array('trim'));
$form->addElement($between3and6);

/**
 * Configure the digits validated field
 * Filters: Trim
 * Validators: Digits
 */
$digits = new Quform_Element('digits');
$digits->addValidator('required');
$digits->addValidator('digits');
$digits->addFilters(array('trim'));
$form->addElement($digits);

/**
 * Configure the alphanumeric validated field
 * Filters: Trim
 * Validators: AlphaNumeric, Required
 */
$alphanumeric = new Quform_Element('alphanumeric');
$alphanumeric->addValidators(array('alphaNumeric', 'required'));
$alphanumeric->addFilters(array('trim'));
$form->addElement($alphanumeric);

/**
 * Configure the alphabet validated field
 * Filters: Trim
 * Validators: Alpha, Required
 */
$alphabet = new Quform_Element('alphabet');
$alphabet->addValidator('required');
$alphabet->addValidator('alpha');
$alphabet->addFilters(array('trim'));
$form->addElement($alphabet);

/**
 * Configure the less than 3 field
 * Filters: Trim
 * Validators: Less than 3, Required
 */
$lessThan3 = new Quform_Element('less_than_3');
$lessThan3->addValidators(array(array('lessThan', array('max' => 3)), 'required'));
$lessThan3->addFilters(array('trim'));
$form->addElement($lessThan3);

/**
 * Configure the greater than 3 field
 * Filters: Trim
 * Validators: Greater than 3
 */
$greaterThan3 = new Quform_Element('greater_than_3');
$greaterThan3->addValidator('required');
$greaterThan3->addValidator('greaterThan', array('min' => 3));
$greaterThan3->addFilters(array('trim'));
$form->addElement($greaterThan3);

/**
 * Configure the word count field
 * Filters: Trim
 * Validators: Word count, Required
 */
$wordCount = new Quform_Element('word_count');
$wordCount->addValidator('required');
$wordCount->addValidator('wordCount', array('min' => 5, 'max' => 10));
$wordCount->addFilters(array('trim'));
$form->addElement($wordCount);

/**
 * Configure the in array field
 * Filters: Trim
 * Validators: In array, Required
*/
$inArray = new Quform_Element('in_array');
$inArray->addValidator('required');
$inArray->addValidator('inArray', array('haystack' => array('theme', 'catcher', 'quform')));
$inArray->addFilters(array('trim'));
$form->addElement($inArray);

/**
 * Configure the regex field
 * Filters: Trim
 * Validators: Regex, Required
*/
$regexElement = new Quform_Element('regex');
$regexElement->addValidator('required');
$regexElement->addValidator('regex', array('pattern' => '/[\d+]/'));
$regexElement->addFilter('trim');
$form->addElement($regexElement);

/**
 * Configure the CAPTCHA field
 * Filters: Trim
 * Validators: Required, Identical
 */
$captcha = new Quform_Element('type_the_word');
$captcha->addFilter('trim');
$captcha->addValidators(array('required', array('identical', array('token' => 'catch'))));
$captcha->setIsHidden(true);
$form->addElement($captcha);

/** END FORM ELEMENT CONFIGURATION **/

function process(Quform $form, array &$config)
{
    // Process the form
    if ($form->isValid($_POST)) {
        // Custom code section #1 - see documentation for examples

        // End custom code section #1

        try {
            $attachments = array();
            $elements = $form->getElements();

            // Process uploaded files
            foreach ($elements as $element) {
                if ($element instanceof Quform_Element_File
                && array_key_exists($element->getName(), $_FILES)
                && is_array($_FILES[$element->getName()])) {
                    $file = $_FILES[$element->getName()];

                    if (is_array($file['error'])) {
                        // Process multiple upload field
                        foreach ($file['error'] as $key => $error) {
                            if ($error === UPLOAD_ERR_OK) {
                                $fileData = array(
                                    'path' => $file['tmp_name'][$key],
                                    'filename' => Quform_Element_File::filterFilename($file['name'][$key]),
                                    'type' => $file['type'][$key],
                                    'size' => $file['size'][$key]
                                );

                                if ($config['saveUploads'] && $element->getSave()) {
                                    $result = Quform_Element_File::saveUpload($config['uploadPath'], $config['uploadUrl'], $fileData, $element);

                                    if (is_array($result)) {
                                        $fileData = $result;
                                    }
                                }

                                if ($element->getAttach()) {
                                    $attachments[] = $fileData;
                                }

                                $element->addFile($fileData);
                            }
                        }
                    } else {
                        // Process single upload field
                        if ($file['error'] === UPLOAD_ERR_OK) {
                            $fileData = array(
                                'path' => $file['tmp_name'],
                                'filename' => Quform_Element_File::filterFilename($file['name']),
                                'type' => $file['type'],
                                'size' => $file['size']
                            );

                            if ($config['saveUploads'] && $element->getSave()) {
                                $result = Quform_Element_File::saveUpload($config['uploadPath'], $config['uploadUrl'], $fileData, $element);

                                if (is_array($result)) {
                                    $fileData = $result;
                                }
                            }

                            if ($element->getAttach()) {
                                $attachments[] = $fileData;
                            }

                            $element->addFile($fileData);
                        }
                    }
                } // element exists in $_FILES
            } // foreach element

            // Save to a MySQL database
            if ($config['database']) {
                /* Step 1: set connection details */
                $db = new mysqli('localhost', 'username', 'password', 'database');

                /* Step 2: set the database table */
                $table = 'table';

                if ($db->connect_errno) {
                    die('Unable to connect to database: ' . $db->connect_error);
                }

                if (strtolower(QUFORM_CHARSET) == 'utf-8') {
                    if (!$db->set_charset('utf8')) {
                        die('Error loading character set utf8 [' . $db->errno . ']: ' . $db->error);
                    }
                }

                /* Step 3: configure the data to save */
                $data = array(
                    'name' => $form->getValue('name'),
                    'email' => $form->getValue('email'),
                    'message' => $form->getValue('message'),
                );

                $columns = implode(", ", array_keys($data));
                $values = array_map(array($db, 'real_escape_string'), array_values($data));
                $values = implode("', '", $values);

                $query = "INSERT INTO `$table` ($columns) VALUES ('$values')";

                if (!$db->query($query)) {
                    die('Query error [' . $db->errno . ']: ' . $db->error . "\n\n" . $query);
                }

                $db->close();
            }

            if ($config['email']) {
                // Get a new PHPMailer instance
                $mailer = Quform::newPHPMailer($config['smtp']);

                // Set the from information
                $from = $form->parseEmailRecipient($config['from']);

                if ($from['email']) {
                    $mailer->setFrom($from['email'], $from['name']);
                }

                // Set the Reply-To header of the email as the submitted email address from the form
                if (!empty($config['replyTo'])) {
                    $replyTo = $form->parseEmailRecipient($config['replyTo']);

                    if ($replyTo['email']) {
                        $mailer->addReplyTo($replyTo['email'], $replyTo['name']);
                    }
                }

                // Set the subject
                $mailer->Subject = $form->replacePlaceholderValues($config['subject']);

                // Set the recipients
                foreach ((array) $config['recipients'] as $recipient) {
                    $mailer->addAddress($recipient);
                }

                // Set the message body HTML
                ob_start();
                include QUFORM_ROOT . $config['emailBody'];
                $mailer->msgHTML(ob_get_clean());

                $mailer->AltBody = 'To view this email please use HTML compatible email software.';

                // Add any attachments
                foreach ($attachments as $attachment) {
                    $mailer->addAttachment($attachment['path'], $attachment['filename'], 'base64', $attachment['type']);
                }

                // Send the notification message
                $mailer->send();
            }

            // Autoreply email
            if ($config['autoreply']) {
                $autoreplyRecipient = $form->parseEmailRecipient($config['autoreplyRecipient']);

                if ($autoreplyRecipient['email']) {
                    // Create the autoreply message
                    $mailer = Quform::newPHPMailer($config['smtp']);

                    // Set the from address
                    $autoreplyFrom = $form->parseEmailRecipient($config['autoreplyFrom']);

                    if ($autoreplyFrom['email']) {
                        $mailer->setFrom($autoreplyFrom['email'], $autoreplyFrom['name']);
                    }

                    // Set the recipient
                    $mailer->addAddress($autoreplyRecipient['email'], $autoreplyRecipient['name']);

                    // Set the subject
                    $mailer->Subject = $form->replacePlaceholderValues($config['autoreplySubject']);

                    // Set the message body HTML
                    ob_start();
                    include QUFORM_ROOT . $config['autoreplyBody'];
                    $mailer->msgHTML(ob_get_clean());

                    $mailer->AltBody = 'To view this email please use HTML compatible email software.';

                    // Send the autoreply
                    $mailer->send();
                }
            }

            // Custom code section #2 - see documentation for examples

            // End custom code section #2
        } catch (Exception $e) {
            if (QUFORM_DEBUG) {
                throw $e;
            }
        }
    } else {
        // Form data failed validation
        return false;
    }

    // Form processed successfully
    return true;
}

if (process($form, $config)) {
    $result = array('type' => 'success');
    if (strlen($config['redirect'])) {
        $result['redirect'] = $config['redirect'];
    } else {
        $result['message'] = $form->replacePlaceholderValues($config['successMessage']);
    }
} else {
    $result = array('type' => 'error', 'error' => $form->getError(), 'elementErrors' => $form->getElementErrors());
}

if (isset($_POST['quform_ajax']) && $_POST['quform_ajax'] == 1) {
    $response = '<textarea>' . Quform::jsonEncode($result) . '</textarea>';
} else {
    if (isset($result['type'], $result['redirect']) && $result['type'] == 'success' && strlen($result['redirect']) && !headers_sent()) {
        header('Location: ' . $result['redirect']);
        exit;
    }

    ob_start();
    require_once 'nojs.php';
    $response = ob_get_clean();
}

echo $response;