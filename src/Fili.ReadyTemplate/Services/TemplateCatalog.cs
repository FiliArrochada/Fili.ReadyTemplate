namespace Fili.ReadyTemplate.Services;

/// <summary>One template file. <see cref="Key"/> is the resource key holding its display name.</summary>
public sealed record TemplateInfo(string File, string Key)
{
    public string Href => $"templates/{File}";
}

/// <summary>A template in a grouping, with the worksheets attached to it.</summary>
public sealed record TemplateNode(TemplateInfo Template, IReadOnlyList<TemplateNode> Children)
{
    public TemplateNode(TemplateInfo template, params TemplateNode[] children) : this(template, (IReadOnlyList<TemplateNode>)children) { }
}

/// <summary>A heading in the "by activity" grouping.</summary>
public sealed record ActivityGroup(string Key, IReadOnlyList<TemplateNode> Children);

/// <summary>A step in the "by suggested sequence" grouping; <see cref="Number"/> is null for "every week".</summary>
public sealed record SequenceStep(int? Number, IReadOnlyList<TemplateNode> Children);

/// <summary>
/// The templates and how the browse page groups them. This is the data behind the 2004 navigation
/// tree, extended with the twelve worksheets that tree never listed. Display names live in the
/// resource files under the keys given here, so the catalog itself is language-neutral.
/// </summary>
public sealed class TemplateCatalog
{
    public static readonly TemplateInfo AllInOne = new("all-in-one.html", "Tpl_AllInOne");
    public static readonly TemplateInfo Overview = new("index.html", "Tpl_Overview");
    public static readonly TemplateInfo Proposal = new("proposal.html", "Tpl_Proposal");
    public static readonly TemplateInfo TargetAndBenefits = new("target-and-benefits.html", "Tpl_TargetAndBenefits");
    public static readonly TemplateInfo Plan = new("plan.html", "Tpl_Plan");
    public static readonly TemplateInfo ResourceNeeds = new("resource-needs.html", "Tpl_ResourceNeeds");
    public static readonly TemplateInfo Risks = new("risks.html", "Tpl_Risks");
    public static readonly TemplateInfo Legal = new("legal.html", "Tpl_Legal");
    public static readonly TemplateInfo QaPlan = new("qa-plan.html", "Tpl_QaPlan");
    public static readonly TemplateInfo UserNeeds = new("user-needs.html", "Tpl_UserNeeds");
    public static readonly TemplateInfo InterviewNotes = new("interview-notes.html", "Tpl_InterviewNotes");
    public static readonly TemplateInfo InterviewChecklist = new("interview-checklist.html", "Tpl_InterviewChecklist");
    public static readonly TemplateInfo Srs = new("srs.html", "Tpl_Srs");
    public static readonly TemplateInfo UseCaseSuite = new("use-case-suite.html", "Tpl_UseCaseSuite");
    public static readonly TemplateInfo UseCases = new("use-cases.html", "Tpl_UseCases");
    public static readonly TemplateInfo UseCaseFormat = new("use-case-format.html", "Tpl_UseCaseFormat");
    public static readonly TemplateInfo FeatureSet = new("feature-set.html", "Tpl_FeatureSet");
    public static readonly TemplateInfo Features = new("features.html", "Tpl_Features");
    public static readonly TemplateInfo FeatureFormat = new("feature-format.html", "Tpl_FeatureFormat");
    public static readonly TemplateInfo Design = new("design.html", "Tpl_Design");
    public static readonly TemplateInfo DesignArchitecture = new("design-architecture.html", "Tpl_DesignArchitecture");
    public static readonly TemplateInfo DesignComponents = new("design-components.html", "Tpl_DesignComponents");
    public static readonly TemplateInfo DesignSrcOrg = new("design-src-org.html", "Tpl_DesignSrcOrg");
    public static readonly TemplateInfo DesignUi = new("design-ui.html", "Tpl_DesignUi");
    public static readonly TemplateInfo DesignPersistence = new("design-persistence.html", "Tpl_DesignPersistence");
    public static readonly TemplateInfo DesignScalability = new("design-scalability.html", "Tpl_DesignScalability");
    public static readonly TemplateInfo DesignSecurity = new("design-security.html", "Tpl_DesignSecurity");
    public static readonly TemplateInfo UserGuide = new("userguide.html", "Tpl_UserGuide");
    public static readonly TemplateInfo TestSuite = new("test-suite.html", "Tpl_TestSuite");
    public static readonly TemplateInfo TestCaseFormat = new("test-case-format.html", "Tpl_TestCaseFormat");
    public static readonly TemplateInfo TestCases = new("test-cases.html", "Tpl_TestCases");
    public static readonly TemplateInfo TestRunSuite = new("test-run-suite.html", "Tpl_TestRunSuite");
    public static readonly TemplateInfo TestRuns = new("test-runs.html", "Tpl_TestRuns");
    public static readonly TemplateInfo ReleaseChecklist = new("release-checklist.html", "Tpl_ReleaseChecklist");
    public static readonly TemplateInfo Install = new("install.html", "Tpl_Install");
    public static readonly TemplateInfo ReleaseNotes = new("release-notes.html", "Tpl_ReleaseNotes");
    public static readonly TemplateInfo DemoScript = new("demo-script.html", "Tpl_DemoScript");
    public static readonly TemplateInfo Faq = new("faq.html", "Tpl_Faq");
    public static readonly TemplateInfo ImplementationNotes = new("implementation-notes.html", "Tpl_ImplementationNotes");
    public static readonly TemplateInfo Glossary = new("glossary.html", "Tpl_Glossary");
    public static readonly TemplateInfo GlossaryStd = new("glossary-std.html", "Tpl_GlossaryStd");
    public static readonly TemplateInfo StatusReport = new("status-report.html", "Tpl_StatusReport");
    public static readonly TemplateInfo ReviewMeetingNotes = new("review-meeting-notes.html", "Tpl_ReviewMeetingNotes");
    public static readonly TemplateInfo ReviewMeetingChecklists = new("review-meeting-checklists.html", "Tpl_ReviewMeetingChecklists");
    public static readonly TemplateInfo Sdm = new("sdm.html", "Tpl_Sdm");
    public static readonly TemplateInfo License = new("readyset-license.html", "Tpl_License");

    /// <summary>Every template, in no particular order; the browse page sorts by localized name.</summary>
    public IReadOnlyList<TemplateInfo> All { get; } =
    [
        AllInOne, Overview, Proposal, TargetAndBenefits, Plan, ResourceNeeds, Risks, Legal, QaPlan,
        UserNeeds, InterviewNotes, InterviewChecklist, Srs, UseCaseSuite, UseCases, UseCaseFormat,
        FeatureSet, Features, FeatureFormat, Design, DesignArchitecture, DesignComponents, DesignSrcOrg,
        DesignUi, DesignPersistence, DesignScalability, DesignSecurity, UserGuide, TestSuite,
        TestCaseFormat, TestCases, TestRunSuite, TestRuns, ReleaseChecklist, Install, ReleaseNotes,
        DemoScript, Faq, ImplementationNotes, Glossary, GlossaryStd, StatusReport, ReviewMeetingNotes,
        ReviewMeetingChecklists, Sdm, License,
    ];

    public IReadOnlyList<ActivityGroup> ByActivity { get; } =
    [
        new("Group_Planning",
        [
            new(Overview),
            new(Proposal, new TemplateNode(TargetAndBenefits)),
            new(Plan, new TemplateNode(ResourceNeeds), new TemplateNode(Risks)),
            new(Legal),
            new(QaPlan),
        ]),
        new("Group_Requirements",
        [
            new(UserNeeds, new TemplateNode(InterviewNotes), new TemplateNode(InterviewChecklist)),
            new(Srs,
                new TemplateNode(UseCaseSuite), new TemplateNode(UseCases), new TemplateNode(UseCaseFormat),
                new TemplateNode(FeatureSet), new TemplateNode(Features), new TemplateNode(FeatureFormat)),
        ]),
        new("Group_Design",
        [
            new(Design,
                new TemplateNode(DesignArchitecture), new TemplateNode(DesignComponents), new TemplateNode(DesignSrcOrg),
                new TemplateNode(DesignUi), new TemplateNode(DesignPersistence), new TemplateNode(DesignScalability),
                new TemplateNode(DesignSecurity)),
        ]),
        new("Group_Implementation",
        [
            new(UserGuide),
            new(TestSuite,
                new TemplateNode(TestCaseFormat), new TemplateNode(TestCases),
                new TemplateNode(TestRunSuite), new TemplateNode(TestRuns)),
        ]),
        new("Group_Deployment",
        [
            new(ReleaseChecklist), new(Install), new(ReleaseNotes), new(DemoScript),
        ]),
        new("Group_Operations",
        [
            new(Faq), new(ImplementationNotes),
        ]),
        new("Group_Continuous",
        [
            new(Glossary, new TemplateNode(GlossaryStd)),
            new(StatusReport),
            new(ReviewMeetingNotes, new TemplateNode(ReviewMeetingChecklists)),
            new(Sdm),
        ]),
    ];

    public IReadOnlyList<SequenceStep> BySequence { get; } =
    [
        new(1,
        [
            new(Overview),
            new(Proposal, new TemplateNode(TargetAndBenefits)),
            new(Plan, new TemplateNode(ResourceNeeds)),
            new(Legal),
            new(Glossary),
        ]),
        new(2,
        [
            new(UserNeeds, new TemplateNode(InterviewNotes)),
            new(Srs, new TemplateNode(UseCaseSuite), new TemplateNode(FeatureSet)),
        ]),
        new(3,
        [
            new(Design,
                new TemplateNode(DesignArchitecture), new TemplateNode(DesignSrcOrg), new TemplateNode(DesignUi),
                new TemplateNode(DesignPersistence), new TemplateNode(DesignSecurity)),
        ]),
        new(4,
        [
            new(QaPlan),
            new(TestSuite, new TemplateNode(TestCaseFormat), new TemplateNode(TestCases)),
        ]),
        new(5, [new(ReviewMeetingNotes)]),
        new(6, [new(ReleaseChecklist)]),
        new(7,
        [
            new(Install), new(ReleaseNotes), new(UserGuide), new(DemoScript), new(Faq), new(ImplementationNotes),
        ]),
        new(null, [new(StatusReport)]),
    ];

    /// <summary>Every resource key the catalog expects the string tables to define.</summary>
    public IEnumerable<string> ResourceKeys()
    {
        foreach (var t in All)
        {
            yield return t.Key;
        }

        foreach (var g in ByActivity)
        {
            yield return g.Key;
        }
    }
}
