// порт на csharp этого https://github.com/nothings/stb/blob/5c205738c191bcb0abc65c4febfa9bd25ff35234/stb_easy_font.h

using System;

namespace Stb
{
    static class EasyFont
    {
        readonly struct InfoStruct(byte a, byte h, byte v) {
            public readonly byte advance = a;
            public readonly byte h_seg = h;
            public readonly byte v_seg = v;
        }
        readonly static InfoStruct[] CharInfo = [
            new(  6,  0,  0 ),  new(  3,  0,  0 ),  new(  5,  1,  1 ),  new ( 7,  1,  4 ),
            new(  7,  3,  7 ),  new(  7,  6, 12 ),  new(  7,  8, 19 ),  new ( 4, 16, 21 ),
            new(  4, 17, 22 ),  new(  4, 19, 23 ),  new( 23, 21, 24 ),  new (23, 22, 31 ),
            new( 20, 23, 34 ),  new( 22, 23, 36 ),  new( 19, 24, 36 ),  new (21, 25, 36 ),
            new(  6, 25, 39 ),  new(  6, 27, 43 ),  new(  6, 28, 45 ),  new ( 6, 30, 49 ),
            new(  6, 33, 53 ),  new(  6, 34, 57 ),  new(  6, 40, 58 ),  new ( 6, 46, 59 ),
            new(  6, 47, 62 ),  new(  6, 55, 64 ),  new( 19, 57, 68 ),  new (20, 59, 68 ),
            new( 21, 61, 69 ),  new( 22, 66, 69 ),  new( 21, 68, 69 ),  new ( 7, 73, 69 ),
            new(  9, 75, 74 ),  new(  6, 78, 81 ),  new(  6, 80, 85 ),  new ( 6, 83, 90 ),
            new(  6, 85, 91 ),  new(  6, 87, 95 ),  new(  6, 90, 96 ),  new ( 7, 92, 97 ),
            new(  6, 96,102 ),  new(  5, 97,106 ),  new(  6, 99,107 ),  new ( 6,100,110 ),
            new(  6,100,115 ),  new(  7,101,116 ),  new(  6,101,121 ),  new ( 6,101,125 ),
            new(  6,102,129 ),  new(  7,103,133 ),  new(  6,104,140 ),  new ( 6,105,145 ),
            new(  7,107,149 ),  new(  6,108,151 ),  new(  7,109,155 ),  new ( 7,109,160 ),
            new(  7,109,165 ),  new(  7,118,167 ),  new(  6,118,172 ),  new ( 4,120,176 ),
            new(  6,122,177 ),  new(  4,122,181 ),  new( 23,124,182 ),  new (22,129,182 ),
            new(  4,130,182 ),  new( 22,131,183 ),  new(  6,133,187 ),  new (22,135,191 ),
            new(  6,137,192 ),  new( 22,139,196 ),  new(  6,144,197 ),  new (22,147,198 ),
            new(  6,150,202 ),  new( 19,151,206 ),  new( 21,152,207 ),  new ( 6,155,209 ),
            new(  3,160,210 ),  new( 23,160,211 ),  new( 22,164,216 ),  new (22,165,220 ),
            new( 22,167,224 ),  new( 22,169,228 ),  new( 21,171,232 ),  new (21,173,233 ),
            new(  5,178,233 ),  new( 22,179,234 ),  new( 23,180,238 ),  new (23,180,243 ),
            new( 23,180,248 ),  new( 22,189,248 ),  new( 22,191,252 ),  new ( 5,196,252 ),
            new(  3,203,252 ),  new(  5,203,253 ),  new( 22,210,253 ),  new ( 0,214,253 ),
        ];

static readonly byte[] HSeg = [
   97,37,69,84,28,51,2,18,10,49,98,41,65,25,81,105,33,9,97,1,97,37,37,36,
    81,10,98,107,3,100,3,99,58,51,4,99,58,8,73,81,10,50,98,8,73,81,4,10,50,
    98,8,25,33,65,81,10,50,17,65,97,25,33,25,49,9,65,20,68,1,65,25,49,41,
    11,105,13,101,76,10,50,10,50,98,11,99,10,98,11,50,99,11,50,11,99,8,57,
    58,3,99,99,107,10,10,11,10,99,11,5,100,41,65,57,41,65,9,17,81,97,3,107,
    9,97,1,97,33,25,9,25,41,100,41,26,82,42,98,27,83,42,98,26,51,82,8,41,
    35,8,10,26,82,114,42,1,114,8,9,73,57,81,41,97,18,8,8,25,26,26,82,26,82,
    26,82,41,25,33,82,26,49,73,35,90,17,81,41,65,57,41,65,25,81,90,114,20,
    84,73,57,41,49,25,33,65,81,9,97,1,97,25,33,65,81,57,33,25,41,25,
];

static readonly byte[] VSeg = [
   4,2,8,10,15,8,15,33,8,15,8,73,82,73,57,41,82,10,82,18,66,10,21,29,1,65,
    27,8,27,9,65,8,10,50,97,74,66,42,10,21,57,41,29,25,14,81,73,57,26,8,8,
    26,66,3,8,8,15,19,21,90,58,26,18,66,18,105,89,28,74,17,8,73,57,26,21,
    8,42,41,42,8,28,22,8,8,30,7,8,8,26,66,21,7,8,8,29,7,7,21,8,8,8,59,7,8,
    8,15,29,8,8,14,7,57,43,10,82,7,7,25,42,25,15,7,25,41,15,21,105,105,29,
    7,57,57,26,21,105,73,97,89,28,97,7,57,58,26,82,18,57,57,74,8,30,6,8,8,
    14,3,58,90,58,11,7,74,43,74,15,2,82,2,42,75,42,10,67,57,41,10,7,2,42,
    74,106,15,2,35,8,8,29,7,8,8,59,35,51,8,8,15,35,30,35,8,8,30,7,8,8,60,
    36,8,45,7,7,36,8,43,8,44,21,8,8,44,35,8,8,43,23,8,8,43,35,8,8,31,21,15,
    20,8,8,28,18,58,89,58,26,21,89,73,89,29,20,8,8,30,7,
];

struct Color (byte[] c)
{
    readonly public byte[] c = c;
}

static uint DrawSegs(float x, float y, Span<byte> segs, bool vertical, Color c, float[] vbuf, uint offset)
{
    int i,j;
    for (i=0; i < segs.Length; ++i) {
        int len = segs[i] & 7;
        x += (segs[i] >> 3) & 1;
        if (len != 0 && offset+12 <= vbuf.Length) {
            float y0 = y + (segs[i]>>4);
            for (j=0; j < 4; ++j) {
                vbuf[offset+0] = x  + (j==1 || j==2 ? (vertical ? 1 : len) : 0);
                vbuf[offset+1] = y0 + (    j >= 2   ? (vertical ? len : 1) : 0);
                vbuf[offset+2] = 0f;
                offset += 3;
            }
        }
    }
    return offset;
}

static float SpacingVal = 0;
static void Spacing(float spacing)
{
   SpacingVal = spacing;
}

public static uint Print(float x, float y, string text, byte[] color, float[] vertex_buffer)
{
    var vbuf = vertex_buffer;
    float start_x = x;
    uint offset = 0;

    Color c = new(color);
    int textIter = 0;

    while (textIter < text.Length && offset < vertex_buffer.Length) {
        if (text[textIter] == '\n') {
            y += 12;
            x = start_x;
        } else {
            byte advance = CharInfo[text[textIter]-32].advance;
            float y_ch = (advance & 16) != 0 ? y+1 : y;
            int h_seg, v_seg, num_h, num_v;
            h_seg = CharInfo[text[textIter]-32  ].h_seg;
            v_seg = CharInfo[text[textIter]-32  ].v_seg;
            num_h = CharInfo[text[textIter]-32+1].h_seg - h_seg;
            num_v = CharInfo[text[textIter]-32+1].v_seg - v_seg;
            offset = DrawSegs(x, y_ch, new Span<byte>(HSeg, h_seg, num_h), false, c, vbuf, offset);
            offset = DrawSegs(x, y_ch, new Span<byte>(VSeg, v_seg, num_v), true, c, vbuf, offset);
            x += advance & 15;
            x += SpacingVal;
        }
        textIter++;
    }
    return offset/12;
}

public static int Width(string text)
{
    float len = 0;
    float max_len = 0;

    int textIter = 0;
    while (textIter < text.Length) {
        if (text[textIter] == '\n') {
            if (len > max_len) max_len = len;
            len = 0;
        } else {
            len += CharInfo[text[textIter]-32].advance & 15;
            len += SpacingVal;
        }
        textIter++;
    }
    if (len > max_len) max_len = len;
    return (int) Math.Ceiling(max_len);
}

public static int Height(string text)
{
    float y = 0;
    bool nonempty_line = false;

    int textIter = 0;
    while (textIter < text.Length) {
        if (text[textIter] == '\n') {
            y += 12;
            nonempty_line = false;
        } else {
            nonempty_line = true;
        }
        textIter++;
    }
    return (int) Math.Ceiling(y + (nonempty_line ? 7 : 0));
}
    }
}
