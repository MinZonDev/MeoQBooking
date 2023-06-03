<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8" />
<title>Quform - Responsive Ajax Contact Form</title>

<link rel="stylesheet" type="text/css" href="css/base.css" />
</head>
<body>
<div class="outside">
    <div class="quform-outer quform-outer-no-js">
        <div class="quform-wrapper quform-cf">
            <div class="quform quform-inner quform-cf">
                <?php if (isset($result) && is_array($result) && array_key_exists('type', $result)) : ?>
                    <?php if ($result['type'] == 'error') : ?>
                        <div class="quform-title quform-cf">Please go back and correct the following errors</div>
                        <?php if (!empty($result['error'])) : ?>
                            <div class="quform-error-message quform-cf">
                                <?php echo $result['error']; ?>
                            </div>
                        <?php endif; ?>
                        <?php if (count($result['elementErrors'])) : ?>
                            <div class="quform-errors-outer quform-cf">
                                <?php foreach ($result['elementErrors'] as $name => $info) : ?>
                                    <?php if (count($info['errors'])) :  ?>
                                        <div class="quform-error-wrap quform-cf">
                                            <div class="quform-error-label"><?php echo Quform::escape($info['label']); ?></div>
                                            <div class="quform-errors quform-cf"><div class="quform-error"><?php echo Quform::escape($info['errors'][0]); ?></div></div>
                                        </div>
                                    <?php endif;?>
                                <?php endforeach; ?>
                            </div>
                        <?php endif; ?>
                    <?php elseif ($result['type'] == 'success') : ?>
                        <div class="quform-success-message quform-cf"><?php echo $result['message']; ?></div>
                    <?php endif; ?>
                <?php endif; ?>
            </div>
         </div>
    </div>
</div>
</body>
</html>