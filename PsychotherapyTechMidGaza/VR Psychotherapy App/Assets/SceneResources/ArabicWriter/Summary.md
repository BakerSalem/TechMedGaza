# Sure, here is a summary of the provided project scripts

>> ValidateOfTextValue.cs
This script is responsible for validating and fixing the input text based on selected language options (English and Arabic). It includes
-- UI References References to the Text component to display the fixed text.
-- Language Options Flags to allow or disallow English and Arabic letters, numbers, and symbols, as well as spaces.
-- Character Limit Minimum and maximum character limits for the input text.
-- Validation Methods Methods to validate the input text, filter characters based on the selected language options, and check if a character is allowed.
-- Fix Method A method to fix the input text for display based on the selected language options.

>> ValidateOfInputFieldValue.cs
This script is responsible for validating and fixing the input text in an InputField based on selected language options (English and Arabic). It includes
-- UI References References to the InputField component and the Text component to display the fixed text.
-- Language Options Flags to allow or disallow English and Arabic letters, numbers, and symbols, as well as spaces.
-- Character Limit Minimum and maximum character limits for the input text.
-- Unity Methods Methods to set the character limit for the InputField and add a listener to validate input as the user types.
-- Validation Methods Methods to validate the input text, filter characters based on the selected language options, and check if a character is allowed.
-- Fix Method A method to fix the input text for display based on the selected language options.

* Both scripts share similar functionality in terms of validating and fixing text based on language options, but ValidateOfTextValue.cs works with a Text component, while ValidateOfInputFieldValue.cs works with an InputField component.