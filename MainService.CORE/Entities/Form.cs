namespace MainService.CORE.Entities
{
    public class Form
    {
        public string FormId { get; set; }  // str_form_id
        public string FormName { get; set; }  // str_form_name
        public string? MenuId { get; set; }  // str_menu_id
        public string? Icon { get; set; }  // str_icon
        public string Link { get; set; }  // str_link
        public string Component { get; set; }  // str_component
        public char IsMenu { get; set; }  // str_isMenu
        public char Active { get; set; }  // str_active
    }
}