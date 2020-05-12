using MiniMdb.Backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace MiniMdb.Backend.ViewModels
{
    //[Validator(typeof(MediaTitleVmValidator))]
    public class MediaTitleVm
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be not empty and less than 100 characters")]
        public string Name { get; set; }

        public MediaTitleType Type { get; set; }

        public long ReleaseDate { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 16, ErrorMessage = "Plot must be at least 16 characters and less than 1000 characters long")]
        public string Plot { get; set; }

        public long AddedAt { get; set; }

        public long UpdatedAt { get; set; }
    }

    //public class MediaTitleVmValidator : AbstractValidator<MediaTitleVm>
    //{
    //    public MediaTitleVmValidator()
    //    {
    //        RuleFor(vm => vm.Name).NotEmpty().WithMessage("Name cannot be empty");
    //        RuleFor(vm => vm.Plot).Length(16, 1000).WithMessage("Plot must be at between 16 and 1000 characters long");

    //    }
    //}
}
