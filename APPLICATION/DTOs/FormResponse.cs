namespace APPLICATION.DTOs
{
    public class FormResponse
    {
        public string str_form_id { get; set; }  // Corresponding to form.FormId
        public string str_form_name { get; set; }  // Corresponding to form.FormName
        public string str_menu_id { get; set; }  // Corresponding to form.MenuId
        public string str_icon { get; set; }  // Corresponding to form.Icon
        public string str_link { get; set; }  // Corresponding to form.Link
        public string str_component { get; set; }  // Corresponding to form.Component
        public char str_isMenu { get; set; }  // Corresponding to form.IsMenu
        public char str_active { get; set; }  // Corresponding to form.Active
    }
}
