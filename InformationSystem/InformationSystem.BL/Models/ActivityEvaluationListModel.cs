namespace InformationSystem.BL.Models;

public record ActivityEvaluationListModel : BaseModel
{
    public ActivityListModel? Activity { get; set; }
    public EvaluationListModel? Evaluation { get; set; }
}