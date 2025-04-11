namespace EasyPoll.Infra.Data.EntityConfig;

internal sealed class VoteEntityTypeConfig : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("Votes");

        builder.HasKey(v => new { v.PollId, v.VoterFingerprint });

        builder.Property(v => v.PollId)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(v => v.VoterFingerprint)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(v => v.OptionIndex)
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(v => v.Timestamp)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne<Poll>()
            .WithMany()
            .HasForeignKey(v => v.PollId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}