using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace learn_english_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLearningContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    English = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    Icon = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Color = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Ipa = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocabularyWords_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWordProgress",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    MasteredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordProgress", x => new { x.UserId, x.WordId });
                    table.ForeignKey(
                        name: "FK_UserWordProgress_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWordProgress_VocabularyWords_WordId",
                        column: x => x.WordId,
                        principalTable: "VocabularyWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "Color", "Description", "English", "Icon", "Key", "Number", "Title" },
                values: new object[,]
                {
                    { "lesson-1", "violet", "Bài 1 gồm các từ vựng về sức khỏe, thăm khám, điều trị, thuốc và chăm sóc răng miệng. Học và ôn tập tất cả trong cùng một bài.", "Medical & healthcare English", "book", "bai1", 1, "Y tế & chăm sóc sức khỏe" },
                    { "lesson-2", "blue", "Bài 2 gồm các họ từ vựng (Noun, Verb, Adj, Adv) về quy trình khám bệnh, nha khoa, bảo hiểm y tế và bệnh viện.", "Word Families: Health, Clinic & Hospital", "medical", "bai2", 2, "Họ từ vựng Y tế & Sức khỏe" }
                });

            migrationBuilder.InsertData(
                table: "VocabularyWords",
                columns: new[] { "Id", "Ipa", "LessonId", "Meaning", "Order", "Text", "Type" },
                values: new object[,]
                {
                    { 1, "/ˈkeəfl/", "lesson-1", "Cẩn thận, cẩn trọng, thận trọng", 1, "careful", "adj." },
                    { 2, "/ˈkeəfəli/", "lesson-1", "Một cách cẩn thận, chu đáo", 2, "carefully", "adv." },
                    { 3, "/prəʊˈæktɪv/", "lesson-1", "Chủ động", 3, "proactive", "adj." },
                    { 4, "/helθ/", "lesson-1", "Sức khỏe", 4, "health", "n." },
                    { 5, "/helθ ˈɪʃuːz/", "lesson-1", "Các vấn đề về sức khỏe", 5, "health issues", "n. phr." },
                    { 6, "/dɪˈtektɪŋ/", "lesson-1", "Phát hiện, nhận biết, dò tìm", 6, "detecting", "v." },
                    { 7, "/ˈsɪmptəmz/", "lesson-1", "Các triệu chứng (bệnh)", 7, "symptoms", "n." },
                    { 8, "/ˈeni ˈsɪmptəmz/", "lesson-1", "Bất kỳ triệu chứng nào", 8, "any symptoms", "n. phr." },
                    { 9, "/ˈɜːli/", "lesson-1", "Sớm", 9, "early", "adv./adj." },
                    { 10, "/triːts/", "lesson-1", "Chữa trị, điều trị, đối xử", 10, "treats", "v." },
                    { 11, "/ˈtriːtɪd/", "lesson-1", "Được điều trị / Đã chữa trị", 11, "treated", "v." },
                    { 12, "/ðə ˈtriːtmənt ˈprəʊses/", "lesson-1", "Quá trình / Quy trình điều trị", 12, "the treatment process", "n. phr." },
                    { 13, "/ðə prɪˈskraɪbd ˈtriːtmənt plæn/", "lesson-1", "Kế hoạch điều trị đã được kê đơn/chỉ định", 13, "the prescribed treatment plan", "n. phr." },
                    { 14, "/ˈfɜːðə(r) ˈtriːtmənt/", "lesson-1", "Việc điều trị chuyên sâu/tiếp theo", 14, "further treatment", "n. phr." },
                    { 15, "/ˈdʒentl/", "lesson-1", "Nhẹ nhàng, dịu dàng", 15, "gentle", "adj." },
                    { 16, "/ˈdʒentli/", "lesson-1", "Một cách nhẹ nhàng, êm ái", 16, "gently", "adv." },
                    { 17, "/ˈpeɪnkɪlə(r)z/", "lesson-1", "Thuốc giảm đau", 17, "painkiller(s)", "n." },
                    { 18, "/ə prɪˈskrɪpʃn fɔː(r) ˈpeɪnkɪləz/", "lesson-1", "Đơn thuốc giảm đau", 18, "a prescription for painkillers", "n. phr." },
                    { 19, "/ˌæntibaɪˈɒtɪks/", "lesson-1", "Thuốc kháng sinh", 19, "antibiotics", "n." },
                    { 20, "/prɪˈskraɪbd/", "lesson-1", "Được kê đơn, được chỉ định", 20, "prescribed", "v." },
                    { 21, "/ə prɪˈskrɪpʃn/", "lesson-1", "Đơn thuốc, toa thuốc", 21, "a prescription", "n." },
                    { 22, "/ˌəʊvə ðə ˈkaʊntə(r) ˈmedsn/", "lesson-1", "Thuốc không cần kê đơn (mua ở quầy)", 22, "over-the-counter medicine", "n. phr." },
                    { 23, "/maɪ ˌmedɪˈkeɪʃnz/", "lesson-1", "Thuốc men của tôi", 23, "my medications", "n. phr." },
                    { 24, "/sɪˈvɪə(r)/", "lesson-1", "Nghiêm trọng, nặng, dữ dội", 24, "severe", "adj." },
                    { 25, "/maɪld/", "lesson-1", "Nhẹ, êm dịu (mức độ nhẹ)", 25, "mild", "adj." },
                    { 26, "/kənˈsʌlt/", "lesson-1", "Tham khảo ý kiến, hội chẩn, tư vấn", 26, "consult", "v." },
                    { 27, "/kənˈsʌlt ðə ˈdɒktə(r)/", "lesson-1", "Tham khảo ý kiến bác sĩ", 27, "consult the doctor", "v. phr." },
                    { 28, "/əˈpɔɪntmənt/", "lesson-1", "Cuộc hẹn, lịch hẹn (với bác sĩ/nha sĩ)", 28, "appointment", "n." },
                    { 29, "/ˈʃedjuːld ən ə ˈpɔɪntmənt/", "lesson-1", "Đã lên lịch một cuộc hẹn", 29, "scheduled an appointment", "v. phr." },
                    { 30, "/ˈhedeɪk/", "lesson-1", "Cơn đau đầu, nhức đầu", 30, "headache", "n." },
                    { 31, "/ˈdentl ˈtʃek ʌp/", "lesson-1", "Khám/kiểm tra răng miệng", 31, "dental check-up", "n. phr." },
                    { 32, "/ðə ˈdentɪst/", "lesson-1", "Nha sĩ, bác sĩ răng hàm mặt", 32, "the dentist", "n." },
                    { 33, "/prəˈfeʃənl/", "lesson-1", "Chuyên nghiệp", 33, "professional", "adj." },
                    { 34, "/les ˈpeɪnfl/", "lesson-1", "Bớt đau đớn hơn, ít đau hơn", 34, "less painful", "adj. phr." },
                    { 35, "/ɪkˈspleɪnd/", "lesson-1", "Đã giải thích, giảng giải", 35, "explained", "v." },
                    { 36, "/ˈklɪəli/", "lesson-1", "Một cách rõ ràng, dễ hiểu", 36, "clearly", "adv." },
                    { 37, "/ˈθʌrəli/", "lesson-1", "Một cách kỹ lưỡng, cặn kẽ", 37, "thoroughly", "adv." },
                    { 38, "/ˈkævətiz/", "lesson-1", "Lỗ sâu răng, các vết sâu răng", 38, "cavity / cavities", "n." },
                    { 39, "/fɪl ə ˈkævəti/", "lesson-1", "Trám/hàn một lỗ sâu", 39, "fill a cavity", "v. phr." },
                    { 40, "/ɡʌm dɪˈziːz/", "lesson-1", "Bệnh về nướu răng, viêm lợi", 40, "gum disease", "n. phr." },
                    { 41, "/liːd tuː/", "lesson-1", "Dẫn đến, gây ra", 41, "lead to", "v. phr." },
                    { 42, "/ˈsɪəriəs ˈdentl ˈprɒbləmz/", "lesson-1", "Các vấn đề nghiêm trọng về răng miệng", 42, "serious dental problems", "n. phr." },
                    { 43, "/ˈsɪəriəs ˈɔːrəl helθ ˈprɒbləmz/", "lesson-1", "Các vấn đề nghiêm trọng về sức khỏe răng miệng", 43, "serious oral health problems", "n. phr." },
                    { 44, "/ɡet ə blʌd test/", "lesson-1", "Làm xét nghiệm máu", 44, "get a blood test", "v. phr." },
                    { 45, "/əˈreɪndʒd tuː/", "lesson-1", "Đã sắp xếp để...", 45, "arranged to", "v. phr." },
                    { 46, "/tiːθ ˈwaɪtnɪŋ ˈɒpʃnz/", "lesson-1", "Các phương pháp tẩy trắng răng", 46, "teeth whitening options", "n. phr." },
                    { 47, "/dɪˈpendɪŋ ɒn ðə kənˈdɪʃn/", "lesson-1", "Tùy thuộc vào tình trạng", 47, "depending on the condition", "phr." },
                    { 48, "/rɪˈliːv/", "lesson-1", "Làm dịu đi, giảm bớt (cơn đau/lo âu)", 48, "relieve", "v." },
                    { 49, "/ˈnaɪðə(r) ˈkɒnstənt nɔː(r) ʌnˈbeərəbl/", "lesson-1", "Không liên tục cũng không quá sức chịu đựng", 49, "neither constant nor unbearable", "phr." },
                    { 50, "/ɪɡˈnɔːd/", "lesson-1", "Bỏ qua, phớt lờ / Bị phớt lờ", 50, "ignored", "v." },
                    { 51, "/skɪpt/", "lesson-1", "Bỏ qua, nhảy cóc, không uống (thuốc)", 51, "skipped", "v." },
                    { 52, "/rɪˈsiːvd/", "lesson-1", "Đã nhận được", 52, "received", "v." },
                    { 53, "/dɪˈtekt/", "lesson-2", "Phát hiện, nhận ra, tìm ra", 1, "detect", "v." },
                    { 54, "/dɪˈtekʃn/", "lesson-2", "Sự phát hiện, việc phát hiện", 2, "detection", "n." },
                    { 55, "/dɪˈtektəbl/", "lesson-2", "Có thể phát hiện, có thể nhận biết", 3, "detectable", "adj." },
                    { 56, "/əˈpɔɪntmənt/", "lesson-2", "Cuộc hẹn, lịch hẹn", 4, "appointment", "n." },
                    { 57, "/meɪk ən əˈpɔɪntmənt/", "lesson-2", "Đặt lịch hẹn, hẹn gặp bác sĩ", 5, "make an appointment", "v. phr." },
                    { 58, "/ˈʃedjuːl ən əˈpɔɪntmənt/", "lesson-2", "Lên lịch hẹn, đặt lịch hẹn", 6, "schedule an appointment", "v. phr." },
                    { 59, "/ˈdaɪəɡnəʊz/", "lesson-2", "Chẩn đoán", 7, "diagnose", "v." },
                    { 60, "/ˌdaɪəɡˈnəʊsɪs/", "lesson-2", "Sự chẩn đoán, việc chẩn đoán", 8, "diagnosis", "n." },
                    { 61, "/ˌdaɪəɡˈnɒstɪk/", "lesson-2", "Thuộc về chẩn đoán", 9, "diagnostic", "adj." },
                    { 62, "/əˈses/", "lesson-2", "Đánh giá, định giá", 10, "assess", "v." },
                    { 63, "/əˈsesmənt/", "lesson-2", "Sự đánh giá, cuộc đánh giá", 11, "assessment", "n." },
                    { 64, "/əˈsesəbl/", "lesson-2", "Có thể đánh giá được", 12, "assessable", "adj." },
                    { 65, "/kɔːz/", "lesson-2", "Gây ra, dẫn đến", 13, "cause", "v." },
                    { 66, "/kɔːz/", "lesson-2", "Nguyên nhân, lý do", 14, "cause", "n." },
                    { 67, "/triːt/", "lesson-2", "Điều trị, chữa trị", 15, "treat", "v." },
                    { 68, "/ˈtriːtmənt/", "lesson-2", "Sự điều trị, việc điều trị, liệu trình", 16, "treatment", "n." },
                    { 69, "/ˈtriːtəbl/", "lesson-2", "Có thể điều trị được", 17, "treatable", "adj." },
                    { 70, "/ˈsɜːdʒəri/", "lesson-2", "Phẫu thuật, ca phẫu thuật", 18, "surgery", "n." },
                    { 71, "/ˈsɜːdʒɪkl/", "lesson-2", "Thuộc về phẫu thuật", 19, "surgical", "adj." },
                    { 72, "/ˈsɜːdʒɪkli/", "lesson-2", "Bằng phẫu thuật", 20, "surgically", "adv." },
                    { 73, "/ˈsɜːdʒən/", "lesson-2", "Bác sĩ phẫu thuật", 21, "surgeon", "n." },
                    { 74, "/prɪˈskraɪb/", "lesson-2", "Kê đơn, kê thuốc", 22, "prescribe", "v." },
                    { 75, "/prɪˈskrɪpʃn/", "lesson-2", "Đơn thuốc, toa thuốc", 23, "prescription", "n." },
                    { 76, "/prɪˈskraɪbd/", "lesson-2", "Được kê đơn, theo chỉ định", 24, "prescribed", "adj." },
                    { 77, "/ɪˈfekt/", "lesson-2", "Tác dụng, hiệu quả, tác động", 25, "effect", "n." },
                    { 78, "/ˈsaɪd ɪfekt/", "lesson-2", "Tác dụng phụ", 26, "side effect", "n. phr." },
                    { 79, "/ɪˈfektɪv/", "lesson-2", "Hiệu quả, có tác dụng", 27, "effective", "adj." },
                    { 80, "/ˌɪntərˈækt/", "lesson-2", "Tương tác, tương tác thuốc", 28, "interact", "v." },
                    { 81, "/ˌɪntərˈækʃn/", "lesson-2", "Sự tương tác, tương tác qua lại", 29, "interaction", "n." },
                    { 82, "/ˌɪntərˈæktɪv/", "lesson-2", "Có tính tương tác", 30, "interactive", "adj." },
                    { 83, "/kənˈsʌlt/", "lesson-2", "Tham khảo ý kiến, hỏi ý kiến", 31, "consult", "v." },
                    { 84, "/ˌkɒnslˈteɪʃn/", "lesson-2", "Cuộc tư vấn, buổi tư vấn, sự hội chẩn", 32, "consultation", "n." },
                    { 85, "/kənˈsʌltətɪv/", "lesson-2", "Mang tính tư vấn, có tính cố vấn", 33, "consultative", "adj." },
                    { 86, "/dɪˈsaɪd/", "lesson-2", "Quyết định", 34, "decide", "v." },
                    { 87, "/dɪˈsɪʒn/", "lesson-2", "Quyết định, sự quyết định", 35, "decision", "n." },
                    { 88, "/dɪˈsaɪsɪv/", "lesson-2", "Mang tính quyết định, dứt khoát", 36, "decisive", "adj." },
                    { 89, "/ˈdentɪst/", "lesson-2", "Nha sĩ, bác sĩ răng hàm mặt", 37, "dentist", "n." },
                    { 90, "/ˈdentl/", "lesson-2", "Thuộc về răng miệng, nha khoa", 38, "dental", "adj." },
                    { 91, "/ˈreɡjələ(r)/", "lesson-2", "Thường xuyên, đều đặn", 39, "regular", "adj." },
                    { 92, "/ˌreɡjuˈlærəti/", "lesson-2", "Sự đều đặn, tính quy củ", 40, "regularity", "n." },
                    { 93, "/ˈreɡjələli/", "lesson-2", "Một cách đều đặn, thường xuyên", 41, "regularly", "adv." },
                    { 94, "/əˈdʒʌst/", "lesson-2", "Điều chỉnh, căn chỉnh", 42, "adjust", "v." },
                    { 95, "/əˈdʒʌstmənt/", "lesson-2", "Sự điều chỉnh, mức điều chỉnh", 43, "adjustment", "n." },
                    { 96, "/əˈdʒʌstəbl/", "lesson-2", "Có thể điều chỉnh được", 44, "adjustable", "adj." },
                    { 97, "/waɪt/", "lesson-2", "Trắng, màu trắng", 45, "white", "adj." },
                    { 98, "/ˈwaɪtn/", "lesson-2", "Làm trắng", 46, "whiten", "v." },
                    { 99, "/ˈwaɪtnɪŋ/", "lesson-2", "Tẩy trắng, việc làm trắng", 47, "whitening", "n./adj." },
                    { 100, "/rɪˈmuːv/", "lesson-2", "Loại bỏ, nhổ răng, lấy ra", 48, "remove", "v." },
                    { 101, "/rɪˈmuːvl/", "lesson-2", "Sự loại bỏ, việc nhổ răng", 49, "removal", "n." },
                    { 102, "/rɪˈmuːvəbl/", "lesson-2", "Có thể tháo rời, tháo lắp được", 50, "removable", "adj." },
                    { 103, "/keə(r)/", "lesson-2", "Sự chăm sóc, chăm sóc", 51, "care", "n." },
                    { 104, "/ˈkeəfl/", "lesson-2", "Cẩn thận, thận trọng", 52, "careful", "adj." },
                    { 105, "/ˈkeəfəli/", "lesson-2", "Một cách cẩn thận, cẩn trọng", 53, "carefully", "adv." },
                    { 106, "/ˈɪrɪteɪt/", "lesson-2", "Gây kích ứng, làm tấy rát", 54, "irritate", "v." },
                    { 107, "/ˌɪrɪˈteɪʃn/", "lesson-2", "Sự kích ứng, tình trạng tấy rát", 55, "irritation", "n." },
                    { 108, "/ˈɪrɪteɪtɪŋ/", "lesson-2", "Gây khó chịu, gây kích ứng", 56, "irritating", "adj." },
                    { 109, "/ɪnˈʃʊərəns/", "lesson-2", "Bảo hiểm", 57, "insurance", "n." },
                    { 110, "/ɪnˈʃʊəd/", "lesson-2", "Được bảo hiểm", 58, "insured", "adj." },
                    { 111, "/ɪnˈʃʊərəbl/", "lesson-2", "Có thể được bảo hiểm", 59, "insurable", "adj." },
                    { 112, "/prəˈtekt/", "lesson-2", "Bảo vệ", 60, "protect", "v." },
                    { 113, "/prəˈtekʃn/", "lesson-2", "Sự bảo vệ", 61, "protection", "n." },
                    { 114, "/prəˈtektɪv/", "lesson-2", "Mang tính bảo vệ, bảo hộ", 62, "protective", "adj." },
                    { 115, "/ˈkɒntrækt/", "lesson-2", "Hợp đồng", 63, "contract", "n." },
                    { 116, "/saɪn ə ˈkɒntrækt/", "lesson-2", "Ký hợp đồng", 64, "sign a contract", "v. phr." },
                    { 117, "/kənˈtræktʃuəl/", "lesson-2", "Thuộc về hợp đồng", 65, "contractual", "adj." },
                    { 118, "/prəˈsiːdʒə(r)/", "lesson-2", "Thủ tục, quy trình", 66, "procedure", "n." },
                    { 119, "/prəˈsiːdʒərəl/", "lesson-2", "Thuộc về thủ tục", 67, "procedural", "adj." },
                    { 120, "/tʃuːz/", "lesson-2", "Chọn, lựa chọn", 68, "choose", "v." },
                    { 121, "/tʃɔɪs/", "lesson-2", "Sự lựa chọn, lựa chọn", 69, "choice", "n." },
                    { 122, "/ˈtʃəʊzn/", "lesson-2", "Đã được chọn, được tuyển chọn", 70, "chosen", "adj." },
                    { 123, "/suːt/", "lesson-2", "Phù hợp, thích hợp với", 71, "suit", "v." },
                    { 124, "/ˈsuːtəbl/", "lesson-2", "Phù hợp, thích hợp", 72, "suitable", "adj." },
                    { 125, "/ˈsuːtəbli/", "lesson-2", "Một cách phù hợp, thích đáng", 73, "suitably", "adv." },
                    { 126, "/ˈæspekt/", "lesson-2", "Khía cạnh, mặt", 74, "aspect", "n." },
                    { 127, "/ədˈmɪt/", "lesson-2", "Nhập viện, tiếp nhận vào viện", 75, "admit", "v." },
                    { 128, "/ədˈmɪʃn/", "lesson-2", "Sự nhập viện, sự tiếp nhận, thủ tục nhập viện", 76, "admission", "n." },
                    { 129, "/ədˈmɪsəbl/", "lesson-2", "Có thể chấp nhận được, hợp lệ", 77, "admissible", "adj." },
                    { 130, "/ˈsætɪsfaɪ/", "lesson-2", "Làm hài lòng, thỏa mãn", 78, "satisfy", "v." },
                    { 131, "/ˌsætɪsˈfækʃn/", "lesson-2", "Sự hài lòng, sự thỏa mãn", 79, "satisfaction", "n." },
                    { 132, "/ˌsætɪsˈfæktəri/", "lesson-2", "Vừa lòng, hài lòng, thỏa đáng", 80, "satisfactory", "adj." },
                    { 133, "/ˌsætɪsˈfæktərəli/", "lesson-2", "Một cách thỏa đáng, vừa ý", 81, "satisfactorily", "adv." },
                    { 134, "/kəmˈpleɪnt/", "lesson-2", "Lời phàn nàn, đơn khiếu nại", 82, "complaint", "n." },
                    { 135, "/kəmˈpleɪn/", "lesson-2", "Phàn nàn, than phiền", 83, "complain", "v." },
                    { 136, "/kəmˈpleɪnɪŋ/", "lesson-2", "Hay phàn nàn, sự phàn nàn", 84, "complaining", "n./adj." },
                    { 137, "/ˈdezɪɡneɪt/", "lesson-2", "Chỉ định, bổ nhiệm", 85, "designate", "v." },
                    { 138, "/ˌdezɪɡˈneɪʃn/", "lesson-2", "Sự chỉ định, sự bổ nhiệm", 86, "designation", "n." },
                    { 139, "/ˈdezɪɡneɪtɪd/", "lesson-2", "Được chỉ định, dành riêng", 87, "designated", "adj." },
                    { 140, "/ˈɔːθəraɪz/", "lesson-2", "Ủy quyền, cho phép", 88, "authorize", "v." },
                    { 141, "/ˌɔːθəraɪˈzeɪʃn/", "lesson-2", "Sự ủy quyền, giấy ủy quyền", 89, "authorization", "n." },
                    { 142, "/ˈɔːθəraɪzd/", "lesson-2", "Được ủy quyền, có thẩm quyền", 90, "authorized", "adj." },
                    { 143, "/aɪˈdentɪfaɪ/", "lesson-2", "Xác định, nhận dạng, xác minh danh tính", 91, "identify", "v." },
                    { 144, "/aɪˌdentɪfɪˈkeɪʃn/", "lesson-2", "Giấy tờ tùy thân, sự nhận dạng", 92, "identification", "n." },
                    { 145, "/aɪˌdentɪˈfaɪəbl/", "lesson-2", "Có thể nhận dạng, có thể nhận biết", 93, "identifiable", "adj." },
                    { 146, "/rɪˈsiːv/", "lesson-2", "Nhận, tiếp nhận", 94, "receive", "v." },
                    { 147, "/rɪˈsepʃn/", "lesson-2", "Khu tiếp nhận, quầy lễ tân, sự tiếp đón", 95, "reception", "n." },
                    { 148, "/rɪˈsiːvd/", "lesson-2", "Được tiếp nhận, được công nhận", 96, "received", "adj." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Key",
                table: "Lessons",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Number",
                table: "Lessons",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWordProgress_WordId",
                table: "UserWordProgress",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyWords_LessonId_Order",
                table: "VocabularyWords",
                columns: new[] { "LessonId", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWordProgress");

            migrationBuilder.DropTable(
                name: "VocabularyWords");

            migrationBuilder.DropTable(
                name: "Lessons");
        }
    }
}
