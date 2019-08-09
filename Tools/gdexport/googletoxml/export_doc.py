import gspread
import codecs
import collections
from xml.sax.saxutils import quoteattr
import os

def parse_docs(args):
    import json
    from oauth2client.service_account import ServiceAccountCredentials
    
    path = os.path.split(__file__)[0]
    fname = path + '/furry-service.json'
    json_key = json.load(open(fname))    
    scope = ['https://spreadsheets.google.com/feeds']

    credentials = ServiceAccountCredentials.from_json_keyfile_name(fname, scope)
    gc = gspread.authorize(credentials)
    print "load data:\n"
    
    doc = gc.open(args.src)

    tabs = {}
    for ws in doc.worksheets():
        if (ws.title != "entry") and (ws.title[0]!="*"):
            print ws.title + " loading"
            values = ws.get_all_values()
            tabs[ws.title] = values

    
    #overwrite -dev tabs
    tabsFixed = {}       

    for name, data in tabs.items():
        if "@" in name:
            continue
        if args.preset:
            tabsFixed[name] = tabs.get(name + "@" + args.preset, data)
        
        tabsFixed[name] = data
        
        
    tabs = tabsFixed
            
            

    tb = sorted(tabs)
    res_s = "<data preset=" + '"' + args.preset + '"' + ">\n"

    for title in tb:
        print title + " parsing"
        page = tabs[title]
        i=0
        cols = len(page[0])
        rows = len(page)

        y=1
        res_s += '\t<'+title

        st = ""
        
        
        fields = collections.OrderedDict()
        
        for x in range(cols):
            name = page[0][x]
            fields[name] = name
        
        names = page[0]
        
        if page[1][0]=="type":
            y=2
            types = page[1]
            for x in range(cols):
                name = names[x]
                tp = types[x]
                
                if "@" in name:
                    continue
                
                if name.startswith("*"):
                    continue
                
                st += ' ' + name + '="' + tp + '"'                
                
                
        res_s += st
        res_s += '>\n'

        while y < rows:
            res_s += '\t\t<item'
            
            for x in range(cols):
                name = names[x]
                
                if name.startswith("*"):
                    continue
                
                if "@" in name:
                    continue                
                
                value = page[y][x]
                
                if args.preset:
                    for n, nm in enumerate(names):
                        if name + "@" + args.preset == nm:
                            v = page[y][n]
                            if v:
                                value = v
                            if v == "@":
                                value = ""
                            break
                
                res_s += ' ' + name + '=' + quoteattr(value)

            res_s += '/>\n'
            
            y += 1

        res_s += '\t</'+title+'>\n'

    res_s += '</data>'

    folder = os.path.split(args.dest)[0]

    try:
        os.makedirs(folder)
    except:
        pass


    
        
    header = codecs.open(args.dest, "w", "utf-8-sig")
    header.write(res_s)
    header.close()
    print "file saved: " + args.dest

if __name__ == "__main__":
    import argparse
    parser = argparse.ArgumentParser(description="export gdoc to xml")
    parser.add_argument("-s", "--src", help = "source spreadsheet", required = True)
    parser.add_argument("-d", "--dest", help = "destination file", required = True)
    parser.add_argument("--preset", help = "compile time preset", required = False, default = "")

    args = parser.parse_args()
    
    try:
        os.remove(args.dest)
    except:
        pass
    
    parse_docs(args)
