namespace Unite.Indices.Entities.Basic;

public interface ISpecimensDataIndex
{
    bool Tissues { get; set; }
    bool Cells { get; set; }
    bool Organoids { get; set; }
    bool Xenografts { get; set; }
}
