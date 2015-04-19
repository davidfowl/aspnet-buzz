export module GitHubModels {
    export class ApiEvent {
        Type: string;
        ID: number;
        Repo: Repo;
        Payload: Payload;
        Actor: Actor;
    }

    export class Repo {
        Name: string;
        Url: string;
        Html_Url: string;
    }

    export class Payload {
        Action: string;
        Number: number;
        Issue: Issue;
        Comment: Comment;
        Commits: Commit[];
        Pull_Request: PullRequest;
        Ref: string;
        Ref_Type: string;
    }

    export class Actor {
        Login: string;
        Avatar_Url: string;
    }

    export class Issue {
        Html_Url: string;
        Title: string;
        User: Actor;
        Number: number;
    }

    export class Comment {
        Pull_Request_Url: string;
        Html_Url: string;
        Body: string;
    }

    export class Commit {
        Sha: string;
        Message: string;
        Distinct: boolean;
        Url: string;
    }

    export class PullRequest {
        Number: number;
        Html_Url: string;
        Title: string;
        Head: Branch;
        Base: Branch;
    }

    export class Branch {
        Ref: string;
    }
}