namespace EasyPoll.Infra.Data.EntityConfig;

internal sealed class PollEntityTypeConfig : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.ToTable("Polls");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Question)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Options)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasMaxLength(20)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<PollStatus>(v)
            )
            .HasDefaultValue(PollStatus.Active)
            .IsRequired();

        builder.Property(p => p.ShowResultsAfterVoteOnly)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasIndex(p => p.ExpiresAt)
            .HasDatabaseName("idx_polls_expires_at");
    }
}